using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using DAF.Core;

namespace DAF.Core.MassTransitBus
{
    public class CommonConsumer<T> : Consumes<T>.All
        where T : class
    {
        private ISubscriber<T> subscriber;

        public CommonConsumer(ISubscriber<T> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void Consume(T message)
        {
            subscriber.OnReceive(message);
        }
    }
}
