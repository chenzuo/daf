using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.Workflow.Models
{
    [DataContract]
    [KnownType(typeof(DateTimePart))]
    [KnownType(typeof(StateType))]
    [KnownType(typeof(FlowStateOperation))]
    [KnownType(typeof(FlowStateIncome))]
    [KnownType(typeof(FlowStateOutcome))]
    [Table("wf_FlowState")]
    public class FlowState
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string StateId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string FlowId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Code { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Name { get; set; }
        [DataMember]
        [StringLength(200)]
        public string Guide { get; set; }

        [DataMember]
        public DateTimePart? IntervalType { get; set; }
        [DataMember]
        public int? ResponseIntervalValue { get; set; }
        [DataMember]
        public int? TreatIntervalValue { get; set; }
        [DataMember]
        public StateType StateType { get; set; }
        [DataMember]
        public FlowResult? Result { get; set; }

        [ForeignKey("FlowId")]
        public virtual BizFlow Flow { get; set; }
        [DataMember]
        public virtual ICollection<FlowStateOperation> Operations { get; set; }
        [DataMember]
        public virtual ICollection<FlowStateIncome> Incomes { get; set; }
        [DataMember]
        public virtual ICollection<FlowStateOutcome> Outcomes { get; set; }
    }
}
