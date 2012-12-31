using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Metadata;
using Autofac;
using Autofac.Core;
using DAF.Core;
using DAF.SSO;
using DAF.Web.Api.Metadata.Providers;

namespace DAF.Web.Api
{
    public class WebApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataAnnotationsModelMetadataProvider2>().As<ModelMetadataProvider>().SingleInstance();
            builder.RegisterType<WebApiAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
