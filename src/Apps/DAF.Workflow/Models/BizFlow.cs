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
    [KnownType(typeof(FlowState))]
    [KnownType(typeof(FlowIncome))]
    [KnownType(typeof(FlowOutcome))]
    [KnownType(typeof(FlowOperation))]
    [KnownType(typeof(NextBizFlow))]
    [Table("wf_BizFlow")]
    public class BizFlow
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string FlowId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Name { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Code { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TargetType { get; set; }
        [DataMember]
        [StringLength(50)]
        public string ClientId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string BizGroup { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Owner { get; set; }
        [DataMember]
        [StringLength(200)]
        public string StartUrl { get; set; }
        [DataMember]
        [StringLength(200)]
        public string DetailUrl { get; set; }
        [DataMember]
        [StringLength(500)]
        public string Guide { get; set; }
        [DataMember]
        public bool StopWhenIncomeRequired { get; set; }
        [DataMember]
        public bool StopWhenOutcomeRequired { get; set; }

        [DataMember]
        public virtual ICollection<FlowState> States { get; set; }
        [DataMember]
        public virtual ICollection<FlowIncome> Incomes { get; set; }
        [DataMember]
        public virtual ICollection<FlowOutcome> Outcomes { get; set; }
        [DataMember]
        public virtual ICollection<FlowOperation> Operations { get; set; }
        [DataMember]
        public virtual ICollection<NextBizFlow> NextBizFlows { get; set; }
    }
}
