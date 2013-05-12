using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Metadata;
using DAF.Core;
using DAF.Core.IOC;
using DAF.SSO;
using DAF.Core.Caching;
using DAF.Web.Api.Metadata.Providers;

namespace DAF.Web.Api
{
    public class WebApiModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<ModelMetadataProvider, DataAnnotationsModelMetadataProvider2>(LifeTimeScope.Singleton);
            builder.RegisterType<IAppEventHandler, WebApiAppEventHandler>();
        }
    }
}
