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
    [KnownType(typeof(FileStatus))]
    [Table("wf_TargetIncome")]
    public class TargetIncome
    {
        [DataMember]
        [Required]
        [Key]
        [StringLength(50)]
        public string TargetIncomeId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string TargetStateId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string IncomeId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Name { get; set; }
        [DataMember]
        [StringLength(200)]
        public string Remark { get; set; }
        [DataMember]
        [StringLength(50)]
        public string UploaderId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string UploaderName { get; set; }
        [DataMember]
        public DateTime? UploadTime { get; set; }
        [DataMember]
        public bool? Verified { get; set; }
        [DataMember]
        [StringLength(50)]
        public string VerifierId { get; set; }
        [DataMember]
        [StringLength(50)]
        public string VerifierName { get; set; }
        [DataMember]
        public DateTime? VerifierTime { get; set; }
        [DataMember]
        [StringLength(50)]
        public string FileType { get; set; }
        [DataMember]
        [StringLength(200)]
        public string FileUrl { get; set; }
        [DataMember]
        public FileStatus FileStatus { get; set; }

        [ForeignKey("TargetStateId")]
        public virtual TargetState TargetState { get; set; }
        [ForeignKey("IncomeId")]
        public virtual FlowIncome Income { get; set; }
    }
}
