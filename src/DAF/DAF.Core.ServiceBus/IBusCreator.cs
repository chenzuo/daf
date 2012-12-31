using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NServiceBus;

namespace DAF.Core.ServiceBus
{
    public interface IBusCreator
    {
        void ConfigBus(IContainer container);
        IBus CreateBus();
    }
}
