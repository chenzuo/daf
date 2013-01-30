using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using DAF.Core;
using DAF.SSO;
using DAF.Core.Caching;

namespace DAF.Web.Mvc
{
    public class WebMvcModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<Configurations.AreaConfigurationProvider>().As<Core.Configurations.IConfigurationProvider>();

            builder.RegisterType<WebMvcAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
