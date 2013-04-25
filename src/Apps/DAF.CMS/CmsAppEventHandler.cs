using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Configurations;

namespace DAF.CMS
{
    public class CmsAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IIocContainer container, object context)
        {
            if (container.IsRegistered<IEnumerable<IConfigurationProvider>>())
                ConfigurationSystem.Install(container.Resolve<IEnumerable<IConfigurationProvider>>());
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
