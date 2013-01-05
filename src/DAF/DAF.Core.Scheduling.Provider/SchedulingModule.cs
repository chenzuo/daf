using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using DAF.Core;
using DAF.Core.Scheduling;
using DAF.Core.Scheduling.Provider;

namespace DAF.Core.Scheduling.Provider
{
    public class SchedulingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ElapseScheduleTimer>().As<IScheduleTimer>().SingleInstance();
            builder.RegisterType<DefaultScheduleManager>().As<IScheduleManager>().SingleInstance();
            builder.RegisterType<TimingTrigger>().As<IScheduleTrigger>().Named<IScheduleTrigger>("TimingTrigger");
            builder.RegisterType<NullOperation>().As<IOperation>().Named<IOperation>("NullOperation");

            builder.RegisterType<ScheduleAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
