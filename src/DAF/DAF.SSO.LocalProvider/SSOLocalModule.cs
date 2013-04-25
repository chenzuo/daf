using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.SSO;
using DAF.Web;
using DAF.Web.Menu;

namespace DAF.SSO.LocalProvider
{
    public class SSOLocalModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IObjectProvider<SSOServer>, WebJsonFileObjectProvider<SSOServer>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("jsonFile", "~/App_Data/sso_server.js");
                    return paras;
                });

            builder.RegisterType<IObjectProvider<SSOClient[]>, WebJsonFileObjectProvider<SSOClient[]>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("jsonFile", "~/App_Data/sso_clients.js");
                    return paras;
                });

            builder.RegisterType<IObjectProvider<SSOClient>, WebJsonFileObjectProvider<SSOClient>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("jsonFile", "~/App_Data/sso_client.js");
                    return paras;
                });

            builder.RegisterType<ISSOConfiguration, DefaultSSOConfiguration>(LiftTimeScope.Singleton);
            builder.RegisterType<Server.ISSOServerProvider, SSOServerProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<Client.ISSOClientProvider, SSOClientProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<Client.IDefaultSessionProvider, Client.DefaultSessionProvider>(LiftTimeScope.Singleton);
        }
    }
}
