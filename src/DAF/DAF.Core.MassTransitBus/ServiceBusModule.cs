using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Autofac;
using MassTransit;

namespace DAF.Core.MassTransitBus
{
    public class ServiceBusModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Publisher>().As<IPublisher>();
            builder.Register(c => ServiceBusFactory.New(sbc =>
            {
                IServiceBusConfigurator config = c.Resolve<IServiceBusConfigurator>();
                config.Config(c, sbc);
            })).As<IServiceBus>()
            .SingleInstance();

            //builder.RegisterType<ServiceBusAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
