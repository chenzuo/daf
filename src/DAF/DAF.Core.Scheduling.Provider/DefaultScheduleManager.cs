using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;
using System.Diagnostics;
using DAF.Core;
using DAF.Core.Generators;
using DAF.Core.Data;
using DAF.Core.Logging;
using DAF.Core.Scheduling;

namespace DAF.Core.Scheduling.Provider
{
    public class DefaultScheduleManager : ProviderBaseWithLogger, IScheduleManager
    {
        private IScheduleTimer timer;
        private IEnumerable<IOperation> operations;
        private IEnumerable<IScheduleTrigger> triggers;
        private ITransactionManager trans;
        private IIdGenerator idGenerator;
        private IRepository<ScheduleTask> repoTask;
        private IRepository<ScheduleTaskTrigger> repoTaskTrigger;
        private IRepository<SheduleTaskExecuteLog> repoTaskLog;

        private List<IScheduleTask> tasks = null;

        public DefaultScheduleManager(IScheduleTimer timer, IEnumerable<IOperation> operations, IEnumerable<IScheduleTrigger> triggers,
            ITransactionManager trans, IIdGenerator idGenerator,
            IRepository<ScheduleTask> repoTask, IRepository<ScheduleTaskTrigger> repoTaskTrigger, IRepository<SheduleTaskExecuteLog> repoTaskLog)
            : base()
        {
            this.timer = timer;
            this.operations = operations;
            this.triggers = triggers;
            this.trans = trans;
            this.idGenerator = idGenerator;
            this.repoTask = repoTask;
            this.repoTaskTrigger = repoTaskTrigger;
            this.repoTaskLog = repoTaskLog;
        }

        public IEnumerable<IOperation> GetOperations()
        {
            return operations;
        }

        public IEnumerable<IScheduleTask> GetTasks()
        {
            if (tasks != null)
                return tasks;
            tasks = new List<IScheduleTask>();
            var query = repoTask.Query(o => o.Enabled == true).ToArray();
            foreach (var t in query)
            {
                if (CheckTaskAvailable(t))
                    tasks.Add(t);
            }
            return tasks;
        }

        public IEnumerable<IScheduleTrigger> GetTriggers()
        {
            return triggers;
        }

        public bool CheckTaskAvailable(IScheduleTask task)
        {
            var op = operations.Any(o => o.Name == task.OperationName);
            if (!op)
                return false;
            var tnames = task.Triggers.Select(o => o.TriggerName).ToArray();
            foreach (var tn in tnames)
            {
                var tt = task.Triggers.First(o => o.TriggerName == tn);
                var t = triggers.Any(o => o.Name == tn);
                if (!t || !tt.Enabled
                    || (tt.BeginTime.HasValue && DateTime.Today < tt.BeginTime.Value)
                    || (tt.EndTime.HasValue && DateTime.Today > tt.EndTime.Value))
                {
                    task.Triggers.Remove(tt);
                }
            }
            return task.Triggers.Count > 0;
        }

        public void Start()
        {
            foreach (var task in GetTasks())
            {
                var op = operations.FirstOrDefault(o => o.Name == task.OperationName);
                if (op != null)
                {
                    op.Start(task.StartParameters.ToDictionary());
                }
            }

            timer.Elapsed += timer_Elapsed;
            int minutes = 5;
            string configMinutes = ConfigurationManager.AppSettings["ScheduleTimerIntervalMinutes"];
            if (!string.IsNullOrEmpty(configMinutes))
            {
                if (!int.TryParse(configMinutes, out minutes))
                    minutes = 5;
            }
            timer.Run(minutes);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            HandleTimerElapsed(e.SignalTime);
        }

        public void HandleTimerElapsed(DateTime signalTime)
        {
            foreach (var task in GetTasks())
            {
                bool isRunning = false;
                foreach (var ti in task.Triggers)
                {
                    var trigger = triggers.FirstOrDefault(o => o.Name == ti.TriggerName);
                    string currTriggerValue = null;
                    if (trigger != null && trigger.IsTriggered(signalTime, ti.LastActiveTime.HasValue ? ti.LastActiveTime.Value : DateTime.MinValue, ti.LastTriggerValue, ti.TriggerParameters.ToDictionary(), out currTriggerValue))
                    {
                        SheduleTaskExecuteLog log = new SheduleTaskExecuteLog();
                        log.LogId = idGenerator.NewId();
                        log.TaskName = task.Name;
                        log.OperationName = task.OperationName;
                        log.TriggerName = trigger.Name;
                        log.ActiveParameters = task.ActiveParameters;
                        log.TriggerTime = signalTime;
                        log.ExecuteTime = DateTime.Now;
                        if (isRunning)
                        {
                            log.Success = false;
                            log.RetryTimes = 0;
                            log.ActiveRemark = "Skiped! Operation is running!";
                            repoTaskLog.Insert(log);
                        }
                        else
                        {
                            var op = operations.FirstOrDefault(o => o.Name == task.OperationName);
                            if (op != null)
                            {
                                try
                                {
                                    Logger.Information(string.Format("Task {0} Trigger {1} Run {2} Triggered at {3:yyyy-MM-dd HH:mm:ss}",
                                        task.Name, trigger.Name, op.Name, signalTime));

                                    isRunning = true;

                                    Stopwatch watch = new Stopwatch();
                                    watch.Start();
                                    op.Active(task.ActiveParameters.ToDictionary());
                                    watch.Stop();

                                    ti.LastActiveTime = signalTime;
                                    ti.LastTriggerValue = currTriggerValue;
                                    repoTaskTrigger.Update(ti as ScheduleTaskTrigger);

                                    log.Success = true;
                                    log.ActiveRemark = string.Format("Operation runed sucessfully elaps {0} seconds.",
                                        watch.Elapsed.Seconds);

                                    repoTaskLog.Insert(log);

                                    isRunning = false;
                                    Logger.Information(string.Format("Task {0} Trigger {1} Run {2} Triggered Sucessfully",
                                           task.Name, trigger.Name, op.Name));
                                }
                                catch (Exception ex)
                                {
                                    op.HandleError(ex);
                                    ti.LastActiveTime = signalTime;
                                    ti.RetryTimes += 1;
                                    repoTaskTrigger.Update(ti as ScheduleTaskTrigger);

                                    log.Success = false;
                                    log.RetryTimes = ti.RetryTimes;
                                    log.Exception = ex.Message;
                                    log.ActiveRemark = "Operation runned with an Exception.";
                                    repoTaskLog.Insert(log);

                                    isRunning = false;
                                    Logger.Information(string.Format("Task {1} Trigger {2} Run {3} Triggered Error:{0}{4}", Environment.NewLine,
                                           task.Name, trigger.Name, op.Name, ex.Message));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            foreach (var task in GetTasks())
            {
                var op = operations.FirstOrDefault(o => o.Name == task.OperationName);
                if (op != null)
                {
                    op.Stop(task.StopParameters.ToDictionary());
                }
            }

            timer.Terminate();
        }
    }
}
