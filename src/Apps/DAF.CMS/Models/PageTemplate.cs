using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_PageTemplate")]
    public class PageTemplate
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string TemplateName { get; set; }
        [StringLength(200)]
        public string TemplatePath { get; set; }
        [StringLength(500)]
        public string AllowContentTypes { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        public virtual ICollection<PageTemplateControl> Controls { get; set; }
    }
}