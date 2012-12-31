using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;
using MassTransit.BusConfigurators;

namespace DAF.Core.MassTransitBus
{
    public class DefaultServiceBusConfigurator : IServiceBusConfigurator
    {
        private string receiveFromEndPoint;

        public DefaultServiceBusConfigurator(string receiveFromEndPoint)
        {
            this.receiveFromEndPoint = receiveFromEndPoint;
        }

        public void Config(IComponentContext container, ServiceBusConfigurator busConfig)
        {
            busConfig.ReceiveFrom(receiveFromEndPoint);
        }
    }
}
