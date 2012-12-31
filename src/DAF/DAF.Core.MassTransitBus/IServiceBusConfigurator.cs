using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;
using MassTransit.BusConfigurators;

namespace DAF.Core.MassTransitBus
{
    public interface IServiceBusConfigurator
    {
        void Config(IComponentContext container, ServiceBusConfigurator busConfig);
    }
}
