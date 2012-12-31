using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using DAF.Core;

namespace DAF.Core.ServiceBus
{
    public class NBusPublisher : IPublisher
    {
        private IBus bus;

        public NBusPublisher(IBusCreator busCreator)
        {
            this.bus = busCreator.CreateBus();
        }

        public void Publish<T>(T msg) where T : class
        {
            bus.Publish(msg);
        }
    }
}
