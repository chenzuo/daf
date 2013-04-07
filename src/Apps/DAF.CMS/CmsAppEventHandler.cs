using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core;
using DAF.Core.Configurations;

namespace DAF.CMS
{
    public class CmsAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
            if (container.IsRegistered<IEnumerable<IConfigurationProvider>>())
                ConfigurationSystem.Install(container.Resolve<IEnumerable<IConfigurationProvider>>());
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
