using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.CMS.Models
{
    [Table("cms_CategoryContent")]
    public class CategoryContent
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string CategoryId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string ContentId { get; set; }

        public int? TopIndex { get; set; }
        public int? HotIndex { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime? OnTime { get; set; }
        public DateTime? OffTime { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("SiteId, ContentId")]
        public virtual Content Content { get; set; }
    }
}