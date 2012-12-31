using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;
using MassTransit.BusConfigurators;

namespace DAF.Core.MassTransitBus
{
    public class MsmqServiceBusConfigurator : IServiceBusConfigurator
    {
        private string receiveFromEndPoint;

        public MsmqServiceBusConfigurator(string receiveFromEndPoint)
        {
            this.receiveFromEndPoint = receiveFromEndPoint;
        }

        public void Config(IComponentContext container, ServiceBusConfigurator busConfig)
        {
            busConfig.UseMsmq();
            busConfig.UseMulticastSubscriptionClient();
            busConfig.VerifyMsmqConfiguration();
            busConfig.VerifyMsDtcConfiguration();
            busConfig.ReceiveFrom(receiveFromEndPoint);
        }
    }
}
