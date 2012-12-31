using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.CMS.Site.Models
{
    [Table("cms_CategoryContent")]
    public class CategoryContent
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string CategoryCode { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string ContentId { get; set; }
        [Required]
        [Key, Column(Order = 3)]
        [StringLength(10)]
        public string Language { get; set; }

        public int? TopIndex { get; set; }
        public int? HotIndex { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime? OnTime { get; set; }
        public DateTime? OffTime { get; set; }

        [ForeignKey("SiteName,ContentId,Language")]
        public virtual Content Content { get; set; }
    }
}