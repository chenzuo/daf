using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NServiceBus;

namespace DAF.Core.ServiceBus
{
    public class NBusAppEventHandler : DAF.Core.IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
            //var busCreator = container.ResolveOptional<IBusCreator>();
            //if (busCreator != null)
            //    busCreator.ConfigBus(container);
            var bus = Configure.With(DAF.Core.Config.TypesToScan)
                 .AsMasterNode()
                 .AutofacBuilder(container)
                 .Log4Net()
                 .XmlSerializer()
                 .MsmqTransport()
                     .IsTransactional(false)
                     .PurgeOnStartup(false)
                 .UnicastBus()
                     .ImpersonateSender(false)
                 .MsmqSubscriptionStorage()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }

        public void OnApplicatoinExit(IContainer container, object context)
        {
        }

        public int ExecuteOrder
        {
            get { return DAF.Core.ExecuteOrder.Earlier; }
        }
    }
}
