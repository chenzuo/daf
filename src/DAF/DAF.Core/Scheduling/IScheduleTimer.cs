using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DAF.Core.Scheduling
{
    public interface IScheduleTimer
    {
        event ElapsedEventHandler Elapsed;
        void Run(int interval);
        void Terminate();
    }
}
