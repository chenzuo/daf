using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_SubSite")]
    public class SubSite
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [StringLength(50)]
        public string SubSiteName { get; set; }

        [StringLength(50)]
        public string Language { get; set; }
        [StringLength(50)]
        public string DateTimeFormat { get; set; }
        [StringLength(50)]
        public string DateFormat { get; set; }
        [StringLength(50)]
        public string TimeFormat { get; set; }
        [StringLength(50)]
        public string CurrencyFormat { get; set; }
        [StringLength(50)]
        public string NumberFormat { get; set; }
        public double TimeZone { get; set; }

        [StringLength(50)]
        public string DefaultTheme { get; set; }
        [StringLength(50)]
        public string DefaultSkin { get; set; }
        [StringLength(50)]
        public string DefaultPageTitle { get; set; }
        [StringLength(500)]
        public string DefaultMetaKeywords { get; set; }
        [StringLength(500)]
        public string DefaultMetaDescription { get; set; }
        [StringLength(50)]
        public string HomePageId { get; set; }


        [ForeignKey("SiteName")]
        public virtual WebSite OwnerSite { get; set; }
        [ForeignKey("HomePageId")]
        public virtual WebPage HomePage { get; set; }
        public virtual ICollection<AppSetting> SiteSettings { get; set; }
        public virtual ICollection<BasicDataItem> BasicDataItems { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<PageTemplate> PageTemplates { get; set; }
        public virtual ICollection<WebPage> Pages { get; set; }
    }
}
