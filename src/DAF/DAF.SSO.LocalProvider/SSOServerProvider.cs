using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Serialization;
using DAF.Core.Data;
using DAF.Core.Localization;
using DAF.Core.Generators;
using DAF.SSO;
using DAF.SSO.Server;
using DAF.SSO.Client;
using DAF.Core.Messaging;

namespace DAF.SSO.LocalProvider
{
    public class SSOServerProvider : ISSOServerProvider
    {
        private IObjectProvider<SSOServer> serverProvider;
        private IObjectProvider<SSOClient[]> clientsProvider;
        private ISSOConfiguration config;
        private IRandomTextGenerator randomGenerator;
        private IIdGenerator idGenerator;
        private IPasswordEncryptionProvider pwdEncryptor;
        private ITransactionManager trans;
        private IRepository<User> repoUser;
        private IRepository<Role> repoRole;
        private IRepository<UserRole> repoUserRole;
        private IRepository<RolePermission> repoRolePermission;
        private IRepository<Permission> repoPermission;
        private IRepository<ServerSession> repoServerSession;

        private List<Tuple<string, PermissionType, List<Permission>>> appProtectedUris;

        public SSOServerProvider(IObjectProvider<SSOClient[]> clientsProvider, IObjectProvider<SSOServer> serverProvider,
            ISSOConfiguration config, IRandomTextGenerator randomGenerator, IIdGenerator idGenerator, IPasswordEncryptionProvider pwdEncryptor,
            ITransactionManager trans,
            IRepository<User> repoUser, IRepository<Role> repoRole,
            IRepository<UserRole> repoUserRole, IRepository<RolePermission> repoRolePermission, IRepository<Permission> repoPermission,
            IRepository<ServerSession> repoServerSession)
        {
            this.serverProvider = serverProvider;
            this.clientsProvider = clientsProvider;
            this.config = config;
            this.randomGenerator = randomGenerator;
            this.idGenerator = idGenerator;
            this.pwdEncryptor = pwdEncryptor;
            this.trans = trans;
            this.repoUser = repoUser;
            this.repoRole = repoRole;
            this.repoUserRole = repoUserRole;
            this.repoRolePermission = repoRolePermission;
            this.repoPermission = repoPermission;
            this.repoServerSession = repoServerSession;
        }

        public SSOClient GetClient(string clientId)
        {
            var obj = clientsProvider.GetObject().FirstOrDefault(o => o.ClientId == clientId);
            return obj;
        }

        public IEncryptionProvider GetClientEncryptor(SSOClient client)
        {
            return new AESEncryptionProvider(client.EncryptKey, client.EncryptScrect);
        }

        public IServerResponse<Session> SignOn(SignOnInfo signOnInfo)
        {
            var client = GetClient(signOnInfo.ClientId);
            var encryptor = GetClientEncryptor(client);
            var hpwd = pwdEncryptor.Encrypt(signOnInfo.Password);

            var obj = repoUser.Query(o => (o.Account == signOnInfo.AccountOrEmailOrMobile || o.Email == signOnInfo.AccountOrEmailOrMobile || o.Mobile == signOnInfo.AccountOrEmailOrMobile)
                && o.Password == hpwd).FirstOrDefault();

            ServerResponse<Session> response = new ServerResponse<Session>();

            if (obj == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.AccountNotFound);
            }
            else
            {
                switch (obj.Status)
                {
                    case DataStatus.Deleted:
                        response.Status = ResponseStatus.Failed;
                        response.Message = DAF.SSO.Resources.Locale(o => o.AccountNotFound);
                        break;
                    case DataStatus.Locked:
                        response.Status = ResponseStatus.Failed;
                        response.Message = DAF.SSO.Resources.Locale(o => o.AccountLocked);
                        break;
                    case DataStatus.ReadOnly:
                        response.Status = ResponseStatus.Failed;
                        response.Message = DAF.SSO.Resources.Locale(o => o.AccountIsReadOnly);
                        break;
                    case DataStatus.Normal:
                    default:
                        response.Status = ResponseStatus.Success;
                        break;
                }
            }
            if (response.Status == ResponseStatus.Success)
            {
                try
                {
                    trans.BeginTransaction();
                    var serverSession = repoServerSession.Query(o => o.SessionId == signOnInfo.SessionId && o.CientId == client.ClientId && o.DeviceId == signOnInfo.DeviceId).FirstOrDefault();
                    if (serverSession == null)
                    {
                        serverSession = new ServerSession()
                      {
                          CientId = client.ClientId,
                          SessionId = signOnInfo.SessionId,
                          FromCientId = null,
                          DeviceId = signOnInfo.DeviceId,
                          DeviceInfo = signOnInfo.DeviceInfo,
                          UserId = obj.UserId,
                          AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength),
                          LastAccessTime = DateTime.Now,
                          AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites)
                      };
                        repoServerSession.Insert(serverSession);
                    }
                    else
                    {
                        if (serverSession.AccessTokenExpiryTime < DateTime.Now)
                        {
                            serverSession.AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength);
                        }
                        serverSession.LastAccessTime = DateTime.Now;
                        serverSession.AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites);

                        repoServerSession.Update(serverSession);
                    }
                    trans.Commit();

                    response.Data = GetClientSession(client, obj, serverSession);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    response.Status = ResponseStatus.Failed;
                    response.Message = ex.Message;
                }
            }
            return response;
        }

        public void SignOff(SessionInfo sessionInfo)
        {
            var serverSession = repoServerSession.Query(o => o.CientId == sessionInfo.ClientId && o.AccessToken == sessionInfo.AccessToken).FirstOrDefault();
            if (serverSession != null)
            {
                try
                {
                    trans.BeginTransaction();
                    repoServerSession.DeleteBatch(o => o.FromCientId == serverSession.CientId && o.FromSessionId == serverSession.SessionId);
                    repoServerSession.Delete(serverSession);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    serverSession.AccessTokenExpiryTime = DateTime.Now;
                    repoServerSession.Update(serverSession);
                }
            }
        }

        public IServerResponse<Session> TransferSignOn(TransferSignOnInfo transferSignOnInfo)
        {
            ServerResponse<Session> response = new ServerResponse<Session>();
            var fromSession = repoServerSession.Query(o => o.CientId == transferSignOnInfo.FromClientId && o.SessionId == transferSignOnInfo.FromSessionId).FirstOrDefault();

            if (fromSession == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.UserSessionNotFound);
            }
            else
            {
                if (fromSession.AccessTokenExpiryTime <= DateTime.Now)
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = DAF.SSO.Resources.Locale(o => o.UserSessionExpired);
                }
                else
                {
                    try
                    {
                        trans.BeginTransaction();
                        var serverSession = repoServerSession.Query(o => o.SessionId == transferSignOnInfo.SessionId && o.CientId == transferSignOnInfo.ClientId && o.DeviceId == transferSignOnInfo.DeviceId).FirstOrDefault();
                        if (serverSession == null)
                        {
                            serverSession = new ServerSession()
                            {
                                CientId = transferSignOnInfo.ClientId,
                                SessionId = transferSignOnInfo.SessionId,
                                FromCientId = transferSignOnInfo.FromClientId,
                                DeviceId = transferSignOnInfo.DeviceId,
                                DeviceInfo = transferSignOnInfo.DeviceInfo,
                                UserId = fromSession.UserId,
                                AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength),
                                LastAccessTime = DateTime.Now,
                                AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites)
                            };
                            repoServerSession.Insert(serverSession);
                        }
                        else
                        {
                            if (serverSession.AccessTokenExpiryTime < DateTime.Now)
                            {
                                serverSession.AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength);
                            }
                            serverSession.LastAccessTime = DateTime.Now;
                            serverSession.AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites);

                            repoServerSession.Update(serverSession);
                        }
                        trans.Commit();
                        var client = GetClient(transferSignOnInfo.ClientId);
                        var obj = repoUser.Query(o => o.UserId == serverSession.UserId).FirstOrDefault();
                        response.Data = GetClientSession(client, obj, serverSession);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        response.Status = ResponseStatus.Failed;
                        response.Message = ex.Message;
                    }
                }
            }

            return response;
        }

        public IServerResponse<Session> Register(RegisterInfo registerInfo)
        {
            var client = GetClient(registerInfo.ClientId);
            var encryptor = GetClientEncryptor(client);
            var hpwd = pwdEncryptor.Encrypt(registerInfo.Password);

            ServerResponse<Session> response = new ServerResponse<Session>();
            bool exists = repoUser.Query(o => o.Account == registerInfo.Account).Any();
            if (exists)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.DuplicatedAccount);
                return response;
            }

            var ssoServer = serverProvider.GetObject();
            var obj = new User()
            {
                UserId = idGenerator.NewId(),
                Account = registerInfo.Account.ToLower(),
                Password = hpwd,
                ClientId = client.ClientId,
                FullName = registerInfo.FullName,
                NickName = registerInfo.NickName,
                Sex = registerInfo.Sex,
                Birthday = registerInfo.Birthday,
                Mobile = registerInfo.Mobile.ToLower(),
                Email = registerInfo.Email.ToLower(),
                Locale = LocaleHelper.Localizer.GetCurrentCultureInfo(),
                TimeZone = 8.0d,
                Theme = "Default",
                Skin = "Default",
                Status = DataStatus.Normal
            };

            if (repoUser.Insert(obj))
            {
                response.Status = ResponseStatus.Success;
                try
                {
                    trans.BeginTransaction();
                    var serverSession = repoServerSession.Query(o => o.SessionId == registerInfo.SessionId && o.CientId == client.ClientId && o.DeviceId == registerInfo.DeviceId).FirstOrDefault();
                    if (serverSession == null)
                    {
                        serverSession = new ServerSession()
                        {
                            CientId = client.ClientId,
                            SessionId = registerInfo.SessionId,
                            FromCientId = client.ClientId,
                            DeviceId = registerInfo.DeviceId,
                            DeviceInfo = registerInfo.DeviceInfo,
                            UserId = obj.UserId,
                            AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength),
                            LastAccessTime = DateTime.Now,
                            AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites)
                        };
                        repoServerSession.Insert(serverSession);
                    }
                    else
                    {
                        if (serverSession.AccessTokenExpiryTime < DateTime.Now)
                        {
                            serverSession.AccessToken = randomGenerator.Generate(config.TokenAllowedChars, config.TokenLength);
                        }
                        serverSession.LastAccessTime = DateTime.Now;
                        serverSession.AccessTokenExpiryTime = DateTime.Now.AddMinutes(config.SessionExpiredTimeOutMunites);

                        repoServerSession.Update(serverSession);
                    }
                    trans.Commit();

                    response.Data = GetClientSession(client, obj, serverSession);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    response.Status = ResponseStatus.Failed;
                    response.Message = ex.Message;
                }
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.RegisterFailed);
            }
            return response;
        }

        public IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo)
        {
            var client = GetClient(changePasswordInfo.ClientId);
            var encryptor = GetClientEncryptor(client);
            var hpwd = pwdEncryptor.Encrypt(changePasswordInfo.OldPassword);

            var obj = repoUser.Query(o => o.UserId == changePasswordInfo.UserId && o.Password == hpwd).FirstOrDefault();

            ServerResponse<Session> response = new ServerResponse<Session>();

            if (obj == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.AccountNotFound);
            }
            else
            {
                if (changePasswordInfo.NewPassword != changePasswordInfo.ConfirmPassword)
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = DAF.SSO.Resources.Locale(o => o.ConfirmPasswordIsNotSameToPassword);
                }
                else
                {
                    obj.Password = pwdEncryptor.Encrypt(changePasswordInfo.NewPassword);
                    if (repoUser.Update(obj))
                    {
                        response.Status = ResponseStatus.Success;
                        response.Message = DAF.SSO.Resources.Locale(o => o.ChangePasswordSuccessfully);
                    }
                    else
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = DAF.Core.Resources.Locale(o => o.SaveFailure);
                    }
                }
            }
            return response;
        }

        public IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo)
        {
            var emailOrMobile = resetPasswordInfo.EmailOrMobile.ToLower();
            var obj = repoUser.Query(o => o.Email == emailOrMobile || o.Mobile == emailOrMobile).FirstOrDefault();

            ServerResponse<Session> response = new ServerResponse<Session>();

            if (obj == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.EmailOrMobileNotFound);
            }
            else
            {
                obj.Password = pwdEncryptor.Encrypt(resetPasswordInfo.NewPassword);
                if (repoUser.Update(obj))
                {
                    // reset successfully, sent user info.
                    ResetPasswordMessage msg = new ResetPasswordMessage()
                    {
                        Account = obj.Account,
                        FullName = obj.FullName,
                        NickName = obj.NickName,
                        NewPassword = resetPasswordInfo.NewPassword
                    };
                    if(resetPasswordInfo.EmailOrMobile == obj.Email)
                    {
                        msg.Email = obj.Email;
                    }
                    else if(resetPasswordInfo.EmailOrMobile == obj.Mobile)
                    {
                        msg.Mobile = obj.Mobile;
                    }
                    if(!string.IsNullOrEmpty(msg.Email) || !string.IsNullOrEmpty(msg.Mobile))
                    {
                        MessageManager.Publish<ResetPasswordMessage>(msg);
                    }

                    response.Status = ResponseStatus.Success;
                    response.Message = DAF.SSO.Resources.Locale(o => o.ChangePasswordSuccessfully);
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = DAF.Core.Resources.Locale(o => o.SaveFailure);
                }
            }
            return response;
        }

        public void ClearExpiredSessions()
        {
            repoServerSession.DeleteBatch(o => o.AccessTokenExpiryTime <= DateTime.Now);
        }

        private List<Tuple<string, PermissionType, List<Permission>>> GetAppProtectedUris()
        {
            if (appProtectedUris == null)
            {
                appProtectedUris = new List<Tuple<string, PermissionType, List<Permission>>>();
                var ps = repoPermission.Query(null).ToArray();
                foreach (var p in ps)
                {
                    var appUris = appProtectedUris.FirstOrDefault(o => o.Item1 == p.ClientId && o.Item2 == p.PermissionType);
                    if (appUris == null)
                    {
                        appUris = new Tuple<string, PermissionType, List<Permission>>(p.ClientId, p.PermissionType, new List<Permission>());
                        appProtectedUris.Add(appUris);

                    }
                    if (!appUris.Item3.Any(o => o.ClientId == p.ClientId && o.PermissionType == p.PermissionType && o.Uri == p.Uri))
                    {
                        appUris.Item3.Add(p);
                    }
                }
            }
            return appProtectedUris;
        }

        private Session GetClientSession(SSOClient client, User user, ServerSession session)
        {
            var userRoles = repoUserRole.Query(o => o.UserId == user.UserId);
            var roles = userRoles.Select(o => o.RoleId).ToArray();
            var rolePermissions = repoRolePermission.Query(o => userRoles.Any(ur => ur.RoleId == o.RoleId)).ToArray();

            var appUris = GetAppProtectedUris();

            var sps = appProtectedUris.Select(o => new SimplePermission()
            {
                ClientId = o.Item1,
                PermissionType = o.Item2,
                ProtectedUris = o.Item3.Select(p => p.Uri).ToArray(),
                AllowedUris = o.Item3.Where(u => rolePermissions.HasPermitted(u)).Select(u => u.Uri).ToArray()
            }).ToArray();

            return new Session()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientBaseUrl = client.BaseUrl,
                SessionId = session.SessionId,
                AccessToken = session.AccessToken,
                User = user.ToUserSession(),
                Roles = roles,

                DeviceId = session.DeviceId,
                DeviceInfo = session.DeviceInfo,

                Theme = string.IsNullOrEmpty(user.Theme) ? "Default" : user.Theme,
                Skin = string.IsNullOrEmpty(user.Skin) ? "Default" : user.Skin,
                Locale = string.IsNullOrEmpty(user.Locale) ? System.Threading.Thread.CurrentThread.CurrentCulture.Name : user.Locale,
                TimeZone = user.TimeZone,

                Permissions = sps
            };
        }
    }
}
