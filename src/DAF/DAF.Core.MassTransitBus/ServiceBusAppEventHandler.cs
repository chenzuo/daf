using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core;

namespace DAF.Core.MassTransitBus
{
    public class ServiceBusAppEventHandler : IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
            
        }

        public void OnApplicatoinExit(IContainer container, object context)
        {
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Earlier; }
        }
    }
}
