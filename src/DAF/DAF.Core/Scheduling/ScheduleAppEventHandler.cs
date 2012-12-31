using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.Scheduling
{
    public class ScheduleAppEventHandler : IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
            var mgr = container.Resolve<IScheduleManager>();
            mgr.Start();
        }

        public void OnApplicatoinExit(IContainer container, object context)
        {
            var mgr = container.Resolve<IScheduleManager>();
            mgr.Stop();
        }

        public int ExecuteOrder
        {
            get { return 20; }
        }
    }
}
