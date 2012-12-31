using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DAF.Core;
using DAF.Core.Security;
using DAF.SSO.Client;

namespace DAF.SSO.WcfProvider
{
    [ServiceContract]
    [ServiceKnownType(typeof(ServerResponse<Session>))]
    public interface IWcfSSOServerService
    {
        [OperationContract]
        IServerResponse<Session> SignOn(SignOnInfo signOnInfo);
        [OperationContract]
        void SignOff(SessionInfo signOffInfo);

        [OperationContract]
        IServerResponse<Session> Register(RegisterInfo registerInfo);
        [OperationContract]
        IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo);
        [OperationContract]
        IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo);

        [OperationContract]
        IServerResponse<Session> TransferSignOn(TransferSignOnInfo transferSignOnInfo);
    }
}
