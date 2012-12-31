using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Autofac;
using DAF.Core.Scheduling;

namespace DAF.Core.Scheduling.Provider
{
    [Table("schedule_Task")]
    public class ScheduleTask : IScheduleTask
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public bool Enabled { get; set; }

        public int RetryTimes { get; set; }

        [StringLength(200)]
        public string StartParameters { get; set; }

        [StringLength(200)]
        public string ActiveParameters { get; set; }

        [StringLength(200)]
        public string StopParameters { get; set; }

        public virtual ICollection<ScheduleTaskTrigger> TaskTriggers { get; set; }

        [NotMapped]
        public ICollection<ITaskTrigger> Triggers
        {
            get
            {
                var tts = new List<ITaskTrigger>();
                if (TaskTriggers != null)
                {
                    tts.AddRange(TaskTriggers);
                }
                return tts;
            }
            set
            {
                if (value != null)
                {
                    if (TaskTriggers == null)
                        TaskTriggers = new List<ScheduleTaskTrigger>();
                    else
                        TaskTriggers.Clear();
                    foreach (var tt in value)
                    {
                        if (tt is ScheduleTaskTrigger)
                        {
                            TaskTriggers.Add((ScheduleTaskTrigger)tt);
                        }
                    }
                }
                else
                {
                    TaskTriggers.Clear();
                }
            }
        }
    }
}
