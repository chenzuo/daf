using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Scheduling
{
    public interface ISheduleTaskExecuteLog
    {
        string LogId { get; set; }
        string TaskName { get; set; }
        string OperationName { get; set; }
        string TriggerName { get; set; }
        string ActiveRemark { get; set; }
        string ActiveParameters { get; set; }
        DateTime TriggerTime { get; set; }
        DateTime ExecuteTime { get; set; }
        int RetryTimes { get; set; }
        string Exception { get; set; }
        bool Success { get; set; }
    }
}
