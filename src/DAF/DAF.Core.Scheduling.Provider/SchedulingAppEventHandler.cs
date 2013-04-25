using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Scheduling;

namespace DAF.Core.Scheduling.Provider
{
    public class SchedulingAppEventHandler : IAppEventHandler
    {
        public void OnApplicationStart(IIocContainer container, object context)
        {
            var mgr = container.Resolve<IScheduleManager>();
            mgr.Start();
        }

        public void OnApplicatoinExit(IIocContainer container, object context)
        {
            var mgr = container.Resolve<IScheduleManager>();
            mgr.Stop();
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Earliest; ; }
        }
    }
}
