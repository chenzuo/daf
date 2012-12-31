using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core.Scheduling;

namespace DAF.Core.Scheduling.Provider
{
    [Table("schedule_TaskTrigger")]
    public class ScheduleTaskTrigger : ITaskTrigger
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string TaskTriggerId { get; set; }
        [Required]
        [StringLength(50)]
        public string TaskName { get; set; }
        [Required]
        [StringLength(50)]
        public string TriggerName { get; set; }
        public bool Enabled { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        [StringLength(200)]
        public string InitTriggerValue { get; set; }
        [StringLength(200)]
        public string LastTriggerValue { get; set; }
        [StringLength(200)]
        public string TriggerParameters { get; set; }
        public DateTime? LastActiveTime { get; set; }
        public int RetryTimes { get; set; }

        [ForeignKey("TaskName")]
        public virtual ScheduleTask ScheduleTask { get; set; }

        [NotMapped]
        public IScheduleTask Task
        {
            get
            {
                return this.ScheduleTask;
            }
            set
            {
                if(value is ScheduleTask)
                    this.ScheduleTask = (ScheduleTask)value;
            }
        }
    }
}
