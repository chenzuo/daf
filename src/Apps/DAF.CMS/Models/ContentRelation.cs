using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.CMS.Models
{
    [Table("cms_ContentRelation")]
    public class ContentRelation
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string RelationId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string ContentId { get; set; }
        [Required]
        [StringLength(50)]
        public string RelatedContentId { get; set; }
        public ContentRelationType RelationType { get; set; }

        [ForeignKey("SiteId, ContentId")]
        public virtual Content Content { get; set; }
        [ForeignKey("SiteId, RelatedContentId")]
        public virtual Content RelatedContent { get; set; }
    }
}