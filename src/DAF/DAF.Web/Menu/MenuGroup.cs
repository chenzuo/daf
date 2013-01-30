using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Web.Menu
{
    [Table("daf_MenuGroup")]
    public class MenuGroup
    {
        public MenuGroup()
        {
            MenuItems = new List<MenuItem>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }

    public static class MenuGroupExtension
    {
        public static void Merge(this MenuGroup to, MenuGroup from)
        {
            List<MenuItem> items = new List<MenuItem>();
            items.AddRange(to.MenuItems.Merge(from.MenuItems));
            to.MenuItems = items;
        }

        public static List<MenuGroup> Merge(this IEnumerable<MenuGroup> groups1, IEnumerable<MenuGroup> groups2)
        {
            var groups = new List<MenuGroup>();
            if (groups1 == null && groups2 == null)
                return groups;

            if (groups1 == null || groups1.Count() <= 0)
            {
                groups.AddRange(groups2);
                return groups;
            }
            if (groups2 == null || groups2.Count() <= 0)
            {
                groups.AddRange(groups1);
                return groups;
            }

            foreach (var g in groups1)
            {
                var g2 = groups2.FirstOrDefault(o => o.Name.ToLower() == g.Name.ToLower());
                if (g2 != null)
                {
                    g.Merge(g2);
                }
                groups.Add(g);
            }

            foreach (var g in groups2)
            {
                if (groups.Any(o => o.Name.ToLower() == g.Name.ToLower()) == false)
                    groups.Add(g);
            }

            return groups;
        }
    }
}
