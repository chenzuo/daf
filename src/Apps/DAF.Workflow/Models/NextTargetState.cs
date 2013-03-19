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
    [Table("wf_NextTargetState")]
    public class NextTargetState
    {
        [DataMember]
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string TargetStateId { get; set; }
        [DataMember]
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string NextTargetStateId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string ParallelTargetStateId { get; set; }

        [ForeignKey("TargetStateId")]
        public virtual TargetState FromTargetState { get; set; }
        [ForeignKey("NextTargetStateId")]
        public virtual TargetState ToTargetState { get; set; }
        [ForeignKey("ParallelTargetStateId")]
        public virtual TargetState ParallelTargetState { get; set; }
    }
}
