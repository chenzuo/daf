using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using Autofac;
using DAF.Core;

namespace DAF.Core.ServiceBus
{
    public class MsmqBusCreator : IBusCreator
    {
        private IBus bus;
        private Configure busConfig;

        public MsmqBusCreator()
        {
        }

        public void ConfigBus(IContainer container)
        {
            busConfig = Configure.With(DAF.Core.Config.TypesToScan)
                .AsMasterNode()
                .AutofacBuilder(container)
                .Log4Net()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(false)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .ImpersonateSender(false)
                .MsmqSubscriptionStorage();
        }

        public IBus CreateBus()
        {
            if (bus == null)
            {
                if (busConfig == null)
                {
                    ConfigBus(IOC.Current);
                }
                bus = busConfig
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
            }

            return bus;
        }
    }
}
