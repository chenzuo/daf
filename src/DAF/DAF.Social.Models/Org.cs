using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Social.Models
{
    [Table("sns_Org")]
    public class Org
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string OrgId { get; set; }
        [Required]
        [StringLength(100)]
        public string OrgName { get; set; }
        [StringLength(50)]
        public string BriefName { get; set; }
        [StringLength(50)]
        public string OrgType { get; set; }
        [StringLength(50)]
        public string Owner { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public DateTime? EstablishDate { get; set; }
        [StringLength(200)]
        public string SiteUrl { get; set; }
        [StringLength(200)]
        public string DetailUrl { get; set; }
    }
}
