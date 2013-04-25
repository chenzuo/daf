using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Web.Mvc;
using DAF.Core.Configurations;

namespace DAF.Web.Mvc
{
    public class WebMvcAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IIocContainer container, object context)
        {
            //if (container.IsRegistered<IEnumerable<IConfigurationProvider>>())
            //    ConfigurationSystem.Install(container.Resolve<IEnumerable<IConfigurationProvider>>());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemedCshtmlViewEngine());
            ModelMetadataProviders.Current = new DAF.Web.Mvc.DefaultModelMetadataProvider();
            //ModelValidatorProviders.Providers.RemoveAt(0);
            //ModelBinders.Binders.DefaultBinder = new DAF.Web.Mvc.DefaultModelBinder();

            if (container is IOC.IIocContainerForMvc)
            {
                DependencyResolver.SetResolver(((IOC.IIocContainerForMvc)container).GetDependencyResolver());
            }
        }

        public void OnApplicatoinExit(IIocContainer container, object context)
        {
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Earliest; }
        }
    }
}
