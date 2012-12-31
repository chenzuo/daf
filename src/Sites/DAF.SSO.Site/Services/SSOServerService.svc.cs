using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Autofac;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Data;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;
using DAF.SSO.WcfProvider;

namespace DAF.SSO.Site.Services
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SSOServerService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SSOServerService.svc 或 SSOServerService.svc.cs，然后开始调试。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SSOServerService : IWcfSSOServerService
    {
        private ISSOServerProvider serverProvider;

        public SSOServerService()
        {
            this.serverProvider = IOC.Current.Resolve<ISSOServerProvider>();
        }

        public IServerResponse<Session> SignOn(SignOnInfo signOnInfo)
        {
            return serverProvider.SignOn(signOnInfo);
        }

        public void SignOff(SessionInfo signOffInfo)
        {
            serverProvider.SignOff(signOffInfo);
        }

        public IServerResponse<Session> Register(RegisterInfo registerInfo)
        {
            return serverProvider.Register(registerInfo);
        }

        public IServerResponse ChangePassword(ChangePasswordInfo changePasswordInfo)
        {
            return serverProvider.ChangePassword(changePasswordInfo);
        }

        public IServerResponse ResetPassword(ResetPasswordInfo resetPasswordInfo)
        {
            return serverProvider.ResetPassword(resetPasswordInfo);
        }

        public IServerResponse<Session> TransferSignOn(TransferSignOnInfo transferSignOnInfo)
        {
            return serverProvider.TransferSignOn(transferSignOnInfo);
        }
    }
}
