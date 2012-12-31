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
    [KnownType(typeof(TargetState))]
    [Table("wf_TargetFlow")]
    public class TargetFlow
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string TargetFlowId { get; set; }
        [DataMember]
        [Required]
        [StringLength(50)]
        public string FlowId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TargetId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string FlowCode { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Title { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Message { get; set; }

        [DataMember]
        public bool HasStarted { get; set; }
        [DataMember]
        public bool HasCompleted { get; set; }
        [DataMember]
        [StringLength(50)]
        public string LastTargetFlowId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string CreatorId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string CreatorName { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public FlowResult? Result { get; set; }

        [ForeignKey("FlowId")]
        public virtual BizFlow Flow { get; set; }
        [ForeignKey("LastTargetFlowId")]
        public virtual TargetFlow LastTargetFlow { get; set; }
        [DataMember]
        public virtual ICollection<TargetState> TreatedStates { get; set; }
    }
}
