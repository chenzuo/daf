using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Caching;

namespace DAF.CMS.Models
{
    [Table("cms_WebPageControl")]
    public class WebPageControl
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string ControlId { get; set; }
        [Required]
        [StringLength(50)]
        public string PageId { get; set; }
        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }
        [Required]
        [StringLength(50)]
        public string Section { get; set; }
        [Required]
        [StringLength(200)]
        public string ControlPath { get; set; }
        [MaxLength]
        public string ControlParas { get; set; }
        [StringLength(50)]
        public string Container { get; set; }
        [MaxLength]
        public string CssStyle { get; set; }
        public int ShowOrder { get; set; }

        public bool Cached { get; set; }
        [StringLength(50)]
        public string CacheKey { get; set; }
        public int? CacheMunites { get; set; }

        [ForeignKey("PageId")]
        public virtual WebPage Page { get; set; }
    }
}