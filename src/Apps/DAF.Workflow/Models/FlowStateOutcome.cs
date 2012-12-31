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
    [Table("wf_FlowStateOutcome")]
    public class FlowStateOutcome
    {
        [DataMember]
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string StateId { get; set; }
        [DataMember]
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string OutcomeId { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }

        [ForeignKey("StateId")]
        public virtual FlowState State { get; set; }
        [ForeignKey("OutcomeId")]
        public virtual FlowOutcome Outcome { get; set; }
    }
}
