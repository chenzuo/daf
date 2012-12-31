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
    [Table("wf_FlowStateOperation")]
    public class FlowStateOperation
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
        public string OperationId { get; set; }

        [ForeignKey("StateId")]
        public virtual FlowState State { get; set; }
        [ForeignKey("OperationId")]
        public virtual FlowOperation Operation { get; set; }
    }
}
