using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.ServiceBus
{
    public class NBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<NBusPublisher>().As<IPublisher>().SingleInstance();
            //builder.RegisterType<MsmqBusCreator>().As<IBusCreator>().SingleInstance();
            //builder.RegisterType<NBusAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
