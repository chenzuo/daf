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
    [Table("wf_FlowIncome")]
    public class FlowIncome
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string IncomeId { get; set; }
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
        public string Description { get; set; }
        [DataMember]
        [StringLength(50)]
        public string FileType { get; set; }
        [DataMember]
        [StringLength(200)]
        public string SampleFileUrl { get; set; }
        [DataMember]
        [StringLength(200)]
        public string UploadUrl { get; set; }

        [ForeignKey("FlowId")]
        public virtual BizFlow Flow { get; set; }
    }
}
