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
    [Table("wf_FlowOperation")]
    public class FlowOperation
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string OperationId { get; set; }
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
        [StringLength(200)]
        public string OperationUrl { get; set; }
        [DataMember]
        [StringLength(200)]
        public string OperationArgs { get; set; }
        [DataMember]
        [StringLength(200)]
        public string PermissionUri { get; set; }
        [DataMember]
        [StringLength(50)]
        public string DefaultNextStateId { get; set; }
        [DataMember]
        public bool CanPlanned { get; set; }
        [DataMember]
        public bool CanCancelled { get; set; }

        [ForeignKey("FlowId")]
        public virtual BizFlow Flow { get; set; }
    }
}
