using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.SSO;

namespace DAF.Social.Models
{
    [Table("sns_Person")]
    public class Person
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string PersonId { get; set; }
        [Required]
        [StringLength(50)]
        public string PersonName { get; set; }
        public Sex? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        [StringLength(50)]
        public string IDCard { get; set; }

        [StringLength(200)]
        public string Photo { get; set; }


        public virtual ICollection<PersonContact> Contacts { get; set; }
        public virtual ICollection<WorkResume> WorkResume { get; set; }
        public virtual ICollection<StudyResume> StudyResume { get; set; }
        public virtual ICollection<PersonCertificate> Certificates { get; set; }
    }
}
