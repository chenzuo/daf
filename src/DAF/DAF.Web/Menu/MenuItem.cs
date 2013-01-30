using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Collections;

namespace DAF.Web.Menu
{
    [Table("daf_MenuItem")]
    public class MenuItem : IOrdered
    {
        public MenuItem()
        {
            Parent = null;
            Children = new List<MenuItem>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string MenuGroupName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
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

        [ForeignKey("MenuGroupName, ParentName")]
        public virtual MenuItem Parent { get; set; }
        [ForeignKey("MenuGroupName")]
        public virtual MenuGroup MenuGroup { get; set; }

        public virtual ICollection<MenuItem> Children { get; set; }
    }

    public static class MenuItemExtension
    {
        public static IEnumerable<MenuItem> Merge(this IEnumerable<MenuItem> items1, IEnumerable<MenuItem> items2)
        {
            return HierarchyHelper.Merge<MenuItem>(items1, items2, o => o.Children,
                (a, b) => a.Name.ToLower() == b.Name.ToLower() && a.LinkUrl.ToLower() == b.LinkUrl.ToLower(),
                (p, c) => { c.Parent = p; p.Children.Add(c); },
                (list, c) => { c.Parent = null; ((ICollection<MenuItem>)list).Remove(c); });
        }
    }
}
