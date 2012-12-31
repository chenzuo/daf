using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.CMS.Site.Models
{
    [Table("cms_ContentRelation")]
    public class ContentRelation
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ContentId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string RelatedContentId { get; set; }
        [Required]
        [Key, Column(Order = 3)]
        [StringLength(10)]
        public string Language { get; set; }
        [Required]
        [Key, Column(Order = 4)]
        [StringLength(50)]
        public string RelationType { get; set; }

        [ForeignKey("SiteName,ContentId,Language")]
        public virtual Content Content { get; set; }
        [ForeignKey("SiteName,RelatedContentId,Language")]
        public virtual Content RelatedContent { get; set; }
    }
}