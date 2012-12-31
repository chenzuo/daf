using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core.Scheduling;

namespace DAF.Core.Scheduling.Provider
{
    [Table("schedule_TaskExecuteLog")]
    public class SheduleTaskExecuteLog : ISheduleTaskExecuteLog
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string LogId { get; set; }
        [Required]
        [StringLength(50)]
        public string TaskName { get; set; }
        [Required]
        [StringLength(50)]
        public string OperationName { get; set; }
        [Required]
        [StringLength(50)]
        public string TriggerName { get; set; }
        [MaxLength]
        public string ActiveRemark { get; set; }
        [StringLength(200)]
        public string ActiveParameters { get; set; }
        public DateTime TriggerTime { get; set; }
        public DateTime ExecuteTime { get; set; }
        public int RetryTimes { get; set; }
        [MaxLength]
        public string Exception { get; set; }
        public bool Success { get; set; }
    }
}
