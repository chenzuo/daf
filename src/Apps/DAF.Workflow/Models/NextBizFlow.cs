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
    [Table("wf_NextBizFlow")]
    public class NextBizFlow
    {
        [DataMember]
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string FlowId { get; set; }
        [DataMember]
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string NextFlowId { get; set; }

        [ForeignKey("FlowId")]
        public virtual BizFlow FromBizFlow { get; set; }
        [ForeignKey("NextFlowId")]
        public virtual BizFlow ToBizFlow { get; set; }
    }
}
