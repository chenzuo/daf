using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Security;

namespace DAF.SSO.Client
{
    public interface ISSOClientProvider
    {
        IEncryptionProvider GetEncryptor();
        IServerResponse SignOn(SignOnInfo signOnInfo);
        IServerResponse TransferSignOn(TransferSignOnInfo transferSignOnInfo);
        void SignOff();

        IServerResponse Register(RegisterInfo registerInfo);
        IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo);
        IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo);

        ISession GetCurrentSession();
        void SaveSession(ISession session);
    }
}
