using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_WebSite")]
    public class WebSite
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string SiteName { get; set; }
        [StringLength(200)]
        public string UrlStartWith { get; set; }
    }
}
