using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DAF.CMS.Site.Models
{
    [Table("sys_AppSetting")]
    public class AppSetting : DAF.Core.IOrdered
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ClientId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [Key, Column(Order = 3)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Caption { get; set; }
        [StringLength(2000)]
        public string Value { get; set; }
        public int ShowOrder { get; set; }
    }
}