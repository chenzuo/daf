using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Caching;

namespace DAF.CMS.Models
{
    [Table("cms_PageTemplateWidget")]
    public class PageTemplateWidget
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string TemplateWidgetId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }
        [Required]
        [StringLength(50)]
        public string Section { get; set; }
        [Required]
        [StringLength(200)]
        public string WidgetPath { get; set; }
        [MaxLength]
        public string WidgetParas { get; set; }
        [StringLength(50)]
        public string Container { get; set; }
        [MaxLength]
        public string CssStyle { get; set; }
        public int ShowOrder { get; set; }

        public bool Cached { get; set; }
        [StringLength(50)]
        public string CacheKey { get; set; }
        public int? CacheMunites { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("SiteId, TemplateName")]
        public virtual PageTemplate Template { get; set; }
    }
}