using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Configurations;

namespace DAF.Web.Api
{
    public class WebApiAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IIocContainer container, object context)
        {
            if (container is IOC.IIocContainerForApi)
            {
                GlobalConfiguration.Configuration.DependencyResolver = ((IOC.IIocContainerForApi)container).GetDependencyResolver();
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
