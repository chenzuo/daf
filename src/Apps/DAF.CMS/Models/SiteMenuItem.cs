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
    [Table("cms_MenuItem")]
    public class SiteMenuItem : IOrdered
    {
        public SiteMenuItem()
        {
            Parent = null;
            Children = new List<SiteMenuItem>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string MenuGroupName { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }
        [StringLength(200)]
        public string Icon { get; set; }
        [StringLength(50)]
        public string Shortcut { get; set; }
        [StringLength(50)]
        public string Tooltip { get; set; }
        [StringLength(200)]
        public string LinkUrl { get; set; }
        [StringLength(200)]
        public string ProtectedUri { get; set; }
        [StringLength(50)]
        public string Target { get; set; }
        public MenuItemType ItemType { get; set; }
        [StringLength(50)]
        public string ParentName { get; set; }
        public int ShowOrder { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("SiteId, MenuGroupName")]
        public virtual SiteMenuGroup MenuGroup { get; set; }
        [ForeignKey("SiteId, MenuGroupName, ParentName")]
        public virtual SiteMenuItem Parent { get; set; }
        public virtual ICollection<SiteMenuItem> Children { get; set; }
    }
}
