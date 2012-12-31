using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Security;
using DAF.SSO.Client;

namespace DAF.SSO.Server
{
    public interface ISSOServerProvider
    {
        SSOClient GetClient(string clientId);

        IEncryptionProvider GetClientEncryptor(SSOClient client);

        IServerResponse<Session> SignOn(SignOnInfo signOnInfo);
        IServerResponse<Session> TransferSignOn(TransferSignOnInfo transferSignOnInfo);
        void SignOff(SessionInfo sessonInfo);

        IServerResponse<Session> Register(RegisterInfo registerInfo);
        IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo);
        IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo);

        void ClearExpiredSessions();
    }
}
