using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core.Messaging;

namespace DAF.Core.ServiceBus
{
    public class NBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NBusSender>().As<IMessageSender>().SingleInstance();
        }
    }
}
