﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_WebPage")]
    public class WebPage
    {
        public WebPage()
        {
            Parent = null;
            Children = new List<WebPage>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string PageId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }
        [StringLength(50)]
        public string CategoryId { get; set; }

        [StringLength(200)]
        public string ShortUrl { get; set; }
        [StringLength(200)]
        public string HtmlUrl { get; set; }
        [StringLength(200)]
        public string MetaKeywords { get; set; }
        [StringLength(200)]
        public string MetaDescription { get; set; }

        [StringLength(50)]
        public string PageTitle { get; set; }
        [StringLength(50)]
        public string HeaderTitle { get; set; }

        public DataStatus Status { get; set; }
        [StringLength(50)]
        public string ParentPageId { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("PageId")]
        public virtual WebPage Parent { get; set; }
        public virtual ICollection<WebPage> Children { get; set; }
        [ForeignKey("SiteId, TemplateName")]
        public virtual PageTemplate Template { get; set; }
        public virtual ICollection<WebPageControl> Controls { get; set; }
    }
}