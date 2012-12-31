using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using DAF.Core;
using DAF.SSO;
using DAF.Web;
using DAF.Web.Menu;

namespace DAF.SSO.WcfProvider
{
    public class SSOClientModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebJsonFileObjectProvider<SSOServer>>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("jsonFile", "~/App_Data/sso_server.js");
                pe.Parameters = new Parameter[] { np };
            }).As<IObjectProvider<SSOServer>>().SingleInstance();
            builder.RegisterType<WebJsonFileObjectProvider<SSOClient[]>>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("jsonFile", "~/App_Data/sso_clients.js");
                pe.Parameters = new Parameter[] { np };
            }).As<IObjectProvider<SSOClient[]>>().SingleInstance();
            builder.RegisterType<WebJsonFileObjectProvider<SSOClient>>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("jsonFile", "~/App_Data/sso_client.js");
                pe.Parameters = new Parameter[] { np };
            }).As<IObjectProvider<SSOClient>>().SingleInstance();

            builder.RegisterType<DefaultSSOConfiguration>().As<ISSOConfiguration>().SingleInstance();
            builder.RegisterType<SSOClientProvider>().As<Client.ISSOClientProvider>().SingleInstance();
        }
    }
}
