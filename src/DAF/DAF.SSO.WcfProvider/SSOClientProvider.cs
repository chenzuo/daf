using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ServiceModel;
using System.Threading;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Serialization;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;

namespace DAF.SSO.WcfProvider
{
    public class SSOClientProvider : ISSOClientProvider
    {
        private const string SessionStorageName = "CurrentSessionInfo";
        private IObjectProvider<SSOClient> ssoClientProvider;

        public SSOClientProvider(IObjectProvider<SSOClient> ssoClientProvider,
            IJsonSerializer jsonConvert)
        {
            this.ssoClientProvider = ssoClientProvider;
        }

        private ChannelFactory<IWcfSSOServerService> CreateChannel()
        {
            return WcfService.CreateChannel<IWcfSSOServerService>("IWcfSSOServerService");
        }

        public IEncryptionProvider GetEncryptor()
        {
            var ssoClient = ssoClientProvider.GetObject();
            return new AESEncryptionProvider(ssoClient.EncryptKey, ssoClient.EncryptScrect);
        }

        public IServerResponse SignOn(SignOnInfo signOnInfo)
        {
            Assert.IsNotNull(signOnInfo);
            Assert.IsStringNotNullOrEmpty(signOnInfo.AccountOrEmailOrMobile);
            Assert.IsStringNotNullOrEmpty(signOnInfo.Password);

            IServerResponse<Session> response = null;

            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                response = p.SignOn(signOnInfo);
                if (response.Status == ResponseStatus.Success && response.Data != null)
                {
                    SaveSession(response.Data);
                }
            });

            return response;
        }

        public void SignOff()
        {
            var sessionInfo = GetCurrentSession();
            if (sessionInfo != null)
            {
                var chanel = CreateChannel();
                chanel.Call(p =>
                {
                    p.SignOff(new SessionInfo()
                    {
                        ClientId = sessionInfo.ClientId,
                        SessionId = sessionInfo.SessionId,
                        AccessToken = sessionInfo.AccessToken
                    });
                });
                SaveSession(null);
            }
        }

        public IServerResponse TransferSignOn(TransferSignOnInfo transferSignOnInfo)
        {
            Assert.IsNotNull(transferSignOnInfo);
            Assert.IsStringNotNullOrEmpty(transferSignOnInfo.ClientId);
            Assert.IsStringNotNullOrEmpty(transferSignOnInfo.SessionId);
            Assert.IsStringNotNullOrEmpty(transferSignOnInfo.FromClientId);
            Assert.IsStringNotNullOrEmpty(transferSignOnInfo.FromSessionId);

            IServerResponse<Session> response = null;

            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                response = p.TransferSignOn(transferSignOnInfo);
                if (response.Status == ResponseStatus.Success && response.Data != null)
                {
                    SaveSession(response.Data);
                }
            });

            return response;
        }

        public IServerResponse Register(RegisterInfo registerInfo)
        {
            Assert.IsNotNull(registerInfo);
            Assert.IsStringNotNullOrEmpty(registerInfo.Password);

            IServerResponse<Session> response = null;

            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                response = p.Register(registerInfo);
                if (response.Status == ResponseStatus.Success && response.Data != null)
                {
                    SaveSession(response.Data);
                }
            });

            return response;
        }

        public IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo)
        {
            Assert.IsNotNull(changePasswordInfo);
            Assert.IsStringNotNullOrEmpty(changePasswordInfo.OldPassword);
            Assert.IsStringNotNullOrEmpty(changePasswordInfo.NewPassword);
            Assert.AreEqual(changePasswordInfo.NewPassword, changePasswordInfo.ConfirmPassword);

            IServerResponse response = null;

            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                response = p.ChangePassword(changePasswordInfo);
            });

            return response;
        }

        public IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo)
        {
            Assert.IsNotNull(resetPasswordInfo);
            Assert.IsStringNotNullOrEmpty(resetPasswordInfo.EmailOrMobile);
            Assert.IsStringNotNullOrEmpty(resetPasswordInfo.NewPassword);

            IServerResponse response = null;

            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                response = p.ResetPassword(resetPasswordInfo);
            });

            return response;
        }

        public ISession GetCurrentSession()
        {
            if (HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session[SessionStorageName] as ISession;
            }
            else
            {
                var storage = Thread.GetNamedDataSlot(SessionStorageName);
                if (storage == null)
                {
                    storage = Thread.AllocateNamedDataSlot(SessionStorageName);
                }
                return Thread.GetData(storage) as ISession;
            }
        }

        public void SaveSession(ISession session)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionStorageName] = session;
            }
            else
            {
                if (session != null)
                {
                    var storage = Thread.GetNamedDataSlot(SessionStorageName);
                    if (storage != null)
                    {
                        Thread.SetData(storage, session);
                    }
                }
                else
                {
                    Thread.FreeNamedDataSlot(SessionStorageName);
                }
            }
        }
    }
}
