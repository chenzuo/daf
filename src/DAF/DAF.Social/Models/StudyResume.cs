using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_StudyResume")]
    public class StudyResume
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string PersonId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string SchoolId { get; set; }
        [StringLength(50)]
        public string GradeName { get; set; }
        [StringLength(50)]
        public string ClassName { get; set; }
        [StringLength(50)]
        public string SubjectName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }
    }
}
