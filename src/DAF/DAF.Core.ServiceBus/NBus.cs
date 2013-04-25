using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using DAF.Core.IOC;

namespace DAF.Core.ServiceBus
{
    public class NBus
    {
        public static IBus Msmq(Func<Configure, Configure> iocConfig = null, Func<Configure, Configure> extraConfig = null)
        {
            var config = Configure.With(DAF.Core.Config.Current.TypesToScan)
                .Log4Net()
                .XmlSerializer();
            if (iocConfig == null)
            {
                config = config.DefaultBuilder();
            }
            else
            {
                config = iocConfig(config);
            }

            config = config.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages"))
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
                .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue)
                .Sagas()
                    .RavenSagaPersister()
                .MsmqTransport()
                    .IsTransactional(true)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .LoadMessageHandlers()
                    .ImpersonateSender(false)
                .MsmqSubscriptionStorage();

            if (extraConfig != null)
                config = extraConfig(config);

            var bus = config.CreateBus()
               .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            return bus;
        }
    }
}
