using System;
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
            Controls = new List<WebPageControl>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string PageId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }

        private string name;
        [Required]
        [StringLength(50)]
        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    name = value.ToLower();
                else
                    name = null;
            }
        }
        [Required]
        [StringLength(50)]
        public string TemplateName { get; set; }
        [StringLength(50)]
        public string CategoryId { get; set; }

        private string shortUrl;
        [StringLength(200)]
        public string ShortUrl
        {
            get { return shortUrl; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    shortUrl = value.ToLower();
                else
                    shortUrl = null;
            }
        }
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
        [MaxLength]
        public string PageLinks { get; set; }
        [MaxLength]
        public string PageCSS { get; set; }
        [MaxLength]
        public string PageJS { get; set; }

        public DataStatus Status { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("SiteId, TemplateName")]
        public virtual PageTemplate Template { get; set; }
        public virtual ICollection<WebPageControl> Controls { get; set; }
    }
}