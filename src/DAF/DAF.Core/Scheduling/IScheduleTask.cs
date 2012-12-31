using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DAF.Core.Scheduling
{
    public interface IScheduleTask
    {
        string Name { get; set; }
        string Description { get; set; }
        string OperationName { get; set; }
        bool Enabled { get; set; }
        int RetryTimes { get; set; }
        string StartParameters { get; set; }
        string ActiveParameters { get; set; }
        string StopParameters { get; set; }

        ICollection<ITaskTrigger> Triggers { get; set; }
    }
}
