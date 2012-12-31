using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;
using MassTransit.BusConfigurators;

namespace DAF.Core.MassTransitBus
{
    public class RabbitMServiceBusConfigurator : IServiceBusConfigurator
    {
        private string receiveFromEndPoint;

        public RabbitMServiceBusConfigurator(string receiveFromEndPoint)
        {
            this.receiveFromEndPoint = receiveFromEndPoint;
        }

        public void Config(IComponentContext container, ServiceBusConfigurator busConfig)
        {
            busConfig.UseRabbitMq();
            busConfig.ReceiveFrom(receiveFromEndPoint);
        }
    }
}
