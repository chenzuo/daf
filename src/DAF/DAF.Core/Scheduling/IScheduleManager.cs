using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Scheduling
{
    public interface IScheduleManager
    {
        IEnumerable<IOperation> GetOperations();
        IEnumerable<IScheduleTask> GetTasks();
        IEnumerable<IScheduleTrigger> GetTriggers();

        bool CheckTaskAvailable(IScheduleTask task);
        void Start();
        void HandleTimerElapsed(DateTime signalTime);
        void Stop();
    }

    public static class IScheduleManagerExtensions
    {
        public static IOperation GetOperation(this IScheduleManager sm, string name)
        {
            var objs = sm.GetOperations();
            return objs.FirstOrDefault(o => o.Name == name);
        }
        public static IScheduleTask GetTask(this IScheduleManager sm, string name)
        {
            var objs = sm.GetTasks();
            return objs.FirstOrDefault(o => o.Name == name);
        }
        public static IScheduleTrigger GetTrigger(this IScheduleManager sm, string name)
        {
            var objs = sm.GetTriggers();
            return objs.FirstOrDefault(o => o.Name == name);
        }
    }
}
