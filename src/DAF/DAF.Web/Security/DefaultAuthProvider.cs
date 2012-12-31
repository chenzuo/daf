using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Security;
using DAF.Core.Generators;
using DAF.Core.Serialization;
using DAF.Web.Host;

namespace DAF.Web.Security
{
    public class DefaultAuthProvider : IAuthProvider
    {
        private const string RandomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const double ResponseTimeOutMinutes = 10.0;
        private const double SessionTimeOutMinutes = 120.0;
        private const int AccessTokenLength = 20;

        private IPasswordEncryptionProvider pwdEncrypt;
        private IAuthEncryptionProvider authEncrypt;
        private IRandomTextGenerator generator;
        private IHostService host;
        private IIdGenerator idGen;
        private IJsonConvert jsonConvert;

        private IRepository<ServerSession> repoSession;
        private IRepository<User> repoUser;
        private IRepository<Role> repoRole;
        private IRepository<UserRole> repoUr;
        private IRepository<RolePrivilege> repoRp;

        public DefaultAuthProvider(IPasswordEncryptionProvider pwdEncrypt, IAuthEncryptionProvider authEncrypt, IRandomTextGenerator generator, IHostService host, IIdGenerator idGen, IJsonConvert jsonConvert
            , IRepository<ServerSession> repoSession, IRepository<User> repoUser, IRepository<Role> repoRole, IRepository<UserRole> repoUr, IRepository<RolePrivilege> repoRp)
        {
            this.pwdEncrypt = pwdEncrypt;
            this.authEncrypt = authEncrypt;
            this.generator = generator;
            this.host = host;
            this.idGen = idGen;
            this.jsonConvert = jsonConvert;

            this.repoSession = repoSession;
            this.repoUser = repoUser;
            this.repoRole = repoRole;
            this.repoUr = repoUr;
            this.repoRp = repoRp;
        }

        public LoginMessage AuthenticateAccount(string site, string device, string deviceId, string sessionId, string account, string password)
        {
            LoginMessage msg = new LoginMessage();
            var user = repoUser.Query(o => o.Account == account && o.Status == DataStatus.Normal).FirstOrDefault();
            if (user == null)
            {
                msg.Status = LoginStatus.AccountNotExists;
                return msg;
            }
            password = pwdEncrypt.Encrypt(password);
            if (user.Password != password)
            {
                msg.Status = LoginStatus.PasswordNotCorrect;
                return msg;
            }

            var siteInfo = host.GetSites().GetSiteByName(site);
            if (siteInfo == null)
            {
                msg.Status = LoginStatus.Exception;
                msg.Message = string.Format("Site {0} is not found.", site);
                return msg;
            }
            string token = generator.Generate(RandomChars, AccessTokenLength);
            var session = repoSession.Query(o => o.SessionId == sessionId).FirstOrDefault();
            if (session == null)
            {
                session = new ServerSession()
                {
                    SessionId = sessionId,
                    SiteName = site,
                    UserId = user.UserId,
                    Device = device,
                    DeviceId = deviceId,
                    AccessToken = token,
                    AccessTokenExpiryTime = DateTime.Now.AddMinutes(SessionTimeOutMinutes),
                    LastAccessTime = DateTime.Now
                };

                repoSession.Insert(session);
            }
            else
            {
                session.AccessToken = token;
                session.UserId = user.UserId;
                session.AccessTokenExpiryTime = DateTime.Now.AddMinutes(SessionTimeOutMinutes);
                session.LastAccessTime = DateTime.Now;

                repoSession.Update(session);
            }

            msg.Status = LoginStatus.Success;
            msg.EncryptedSession = GetEncryptedLocalSession(siteInfo, user, session);
            return msg;
        }

        public LoginMessage GetSession(string site, string sessionId)
        {
            LoginMessage msg = new LoginMessage();
            var siteInfo = host.GetSites().GetSiteByName(site);
            if (siteInfo == null)
            {
                msg.Status = LoginStatus.Exception;
                msg.Message = string.Format("Site {0} is not found.", site);
                return msg;
            }
            var session = repoSession.Query(o => o.SessionId == sessionId).FirstOrDefault();
            if (session == null)
            {
                msg.Status = LoginStatus.Exception;
                msg.Message = string.Format("Session with Id {0} not found! Please login again.", sessionId);
                return msg;
            }
            if (session.LastAccessTime > DateTime.Now.AddMinutes(SessionTimeOutMinutes))
            {
                msg.Status = LoginStatus.Exception;
                msg.Message = string.Format("Session with Id {0} expired! Please login again.", sessionId);
                return msg;
            }
            var user = repoUser.Query(o => o.UserId == session.UserId && o.Status == DataStatus.Normal).FirstOrDefault();
            if (user == null)
            {
                msg.Status = LoginStatus.AccountNotExists;
                return msg;
            }
            msg.Status = LoginStatus.Success;
            msg.EncryptedSession = GetEncryptedLocalSession(siteInfo, user, session);
            return msg;
        }

        public void ClearSession(string site, string sessionId, string token)
        {
            repoSession.DeleteBatch(o => o.SiteName == site && o.SessionId == sessionId && o.AccessToken == token);
        }

        public void UpdateSessionTime(string site, string sessionId, string token)
        {
            var session = repoSession.Query(o => o.SiteName == site && o.SessionId == sessionId && o.AccessToken == token).FirstOrDefault();
            if (session != null)
            {
                session.LastAccessTime = DateTime.Now;
                repoSession.Update(session);
            }
        }

        private string GetEncryptedLocalSession(SiteInfo siteInfo, User user, ServerSession session)
        {
            var lsession = GetLocalSession(siteInfo, user, session);
            var sessionStr = jsonConvert.Serialize(lsession);
            authEncrypt.Key = siteInfo.EncryptKey;
            authEncrypt.IV = siteInfo.EncryptSecret;
            return authEncrypt.Encrypt(sessionStr);
        }

        private LocalSession GetLocalSession(SiteInfo siteInfo, User user, ServerSession session)
        {
            var userRoles = repoUr.Query(o => o.UserId == user.UserId);
            var roles = userRoles.Select(o => o.RoleId).ToArray();
            var pemissions = repoRp.Query(o => userRoles.Any(ur => ur.RoleId == o.RoleId)).ToArray();

            List<AppPrivilege> aps = new List<AppPrivilege>();
            foreach (var per in pemissions)
            {
                AppPrivilege ap = new AppPrivilege()
                {
                    AppName = per.AppName
                };
                var app = host.GetApps().GetAppByName(per.AppName);
                ap.ProtectedUris = app.ProtectedUris.Select(o => o.Uri.ToLower()).ToArray();
                ap.AllowedUris = app.ProtectedUris.Where(o =>
                {
                    if (o.Position > 0 && o.Position < per.Permissions.Length)
                    {
                        return per.Permissions[o.Position] == '1';
                    }
                    return false;
                }).Select(o => o.Uri.ToLower()).ToArray();
                aps.Add(ap);
            }

            var lsession = new LocalSession()
            {
                SiteName = session.SiteName,
                SessionId = session.SessionId,
                AccessToken = session.AccessToken,
                User = user.ToUserSession(),
                Roles = roles,
                Privileges = aps.ToArray(),

                Theme = user.Theme,
                Skin = user.Skin,
                Locale = user.Locale,
                TimeZone = user.TimeZone
            };

            return lsession;
        }
    }
}
