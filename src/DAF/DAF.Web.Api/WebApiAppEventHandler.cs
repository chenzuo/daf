using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DAF.Core;
using DAF.Core.Configurations;

namespace DAF.Web.Api
{
    public class WebApiAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
             GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public void OnApplicatoinExit(IContainer container, object context)
        {
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Earliest; }
        }
    }
}
