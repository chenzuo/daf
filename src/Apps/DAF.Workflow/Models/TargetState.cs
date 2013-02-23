using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Workflow.Models
{
    [DataContract]
    [KnownType(typeof(StateStatus))]
    [KnownType(typeof(TargetIncome))]
    [KnownType(typeof(TargetOutcome))]
    [Table("wf_TargetState")]
    public class TargetState
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string TargetStateId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TargetFlowId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string StateId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string OperationId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Title { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Message { get; set; }

        [DataMember]
        public DateTime? ResponseExpiryTime { get; set; }
        [DataMember]
        public DateTime? TreatExpiryTime { get; set; }

        [DataMember]
        [StringLength(50)]
        public string ResponsorId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string ResponsorName { get; set; }
        [DataMember]
        public DateTime? ResponseTime { get; set; }
        [DataMember]
        [StringLength(50)]
        public string PlannerId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string PlannerName { get; set; }
        [DataMember]
        public DateTime? PlanTreatTime { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TreaterId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TreaterName { get; set; }
        [DataMember]
        public DateTime? TreatTime { get; set; }
        [DataMember]
        [StringLength(50)]
        public string OperatorId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string OperatorName { get; set; }
        [DataMember]
        public DateTime? OperateTime { get; set; }

        [DataMember]
        public StateStatus StateStatus { get; set; }

        [ForeignKey("TargetFlowId")]
        public virtual TargetFlow TargetFlow { get; set; }
        [DataMember]
        public virtual ICollection<TargetIncome> TargetIncomes { get; set; }
        [DataMember]
        public virtual ICollection<TargetOutcome> TargetOutcomes { get; set; }

        public virtual ICollection<NextTargetState> FromTargetStates { get; set; }
        public virtual ICollection<NextTargetState> ToTargetStates { get; set; }
        [ForeignKey("OperationId")]
        public virtual FlowOperation Operation { get; set; }
        [ForeignKey("StateId")]
        public virtual FlowState State { get; set; }
    }
}
