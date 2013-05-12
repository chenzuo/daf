using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Scheduling;
using DAF.Core.Scheduling.Provider;

namespace DAF.Core.Scheduling.Provider
{
    public class SchedulingModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IScheduleTimer, ElapseScheduleTimer>(LifeTimeScope.Singleton);
            builder.RegisterType<IScheduleManager, DefaultScheduleManager>(LifeTimeScope.Singleton);
            builder.RegisterType<IScheduleTrigger, TimingTrigger>(name: "TimingTrigger");
            builder.RegisterType<IOperation, NullOperation>(name: "NullOperation");

            builder.RegisterType<IAppEventHandler, ScheduleAppEventHandler>();
        }
    }
}
