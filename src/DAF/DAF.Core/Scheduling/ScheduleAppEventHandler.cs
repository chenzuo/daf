using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;

namespace DAF.Core.Scheduling
{
    public class ScheduleAppEventHandler : IAppEventHandler
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
            get { return 20; }
        }
    }
}
