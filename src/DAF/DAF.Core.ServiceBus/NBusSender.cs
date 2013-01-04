using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using DAF.Core;
using DAF.Core.Messaging;

namespace DAF.Core.ServiceBus
{
    public class NBusSender : IMessageSender
    {
        private IBus bus;

        public NBusSender(IBus bus)
        {
            this.bus = bus;
        }

        public void Send(params object[] msgs)
        {
            bus.Send(msgs);
        }
    }
}
