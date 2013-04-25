using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.SSO;
using DAF.Core.Caching;

namespace DAF.Web.Mvc
{
    public class WebMvcModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            //builder.RegisterType<Core.Configurations.IConfigurationProvider, Configurations.AreaConfigurationProvider>();

            builder.RegisterType<Core.Localization.ILocalizer, Localization.JsonLocalizer>(LiftTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("paths", "Localization, Areas/*/Localization");
                    return paras;
                });

            builder.RegisterType<IAppEventHandler, WebMvcAppEventHandler>();
        }
    }
}
