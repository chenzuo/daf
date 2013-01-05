using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_PersonCertificate")]
    public class PersonCertificate
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string CertificateNo { get; set; }
        [Required]
        [StringLength(50)]
        public string PersonId { get; set; }
        [StringLength(50)]
        public string CertificateName { get; set; }
        [StringLength(50)]
        public string Level { get; set; }
        [StringLength(200)]
        public string IssueOrgName { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
