using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Messaging;
using DAF.Core.IOC;

namespace DAF.Core.ServiceBus
{
    public class NBusModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IMessageSender, NBusSender>(LifeTimeScope.Singleton);
        }
    }
}
