using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Web.Menu;

namespace DAF.CMS.Models
{
    [Table("cms_MenuGroup")]
    public class SiteMenuGroup
    {
        public SiteMenuGroup()
        {
            MenuItems = new List<SiteMenuItem>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        public virtual ICollection<SiteMenuItem> MenuItems { get; set; }
    }
}
