using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Scheduling
{
    public interface ITaskTrigger
    {
        string TaskName { get; set; }
        string TriggerName { get; set; }
        bool Enabled { get; set; }
        DateTime? BeginTime { get; set; }
        DateTime? EndTime { get; set; }
        string InitTriggerValue { get; set; }
        string LastTriggerValue { get; set; }
        string TriggerParameters { get; set; }
        DateTime? LastActiveTime { get; set; }
        int RetryTimes { get; set; }

        IScheduleTask Task { get; set; }
    }
}
