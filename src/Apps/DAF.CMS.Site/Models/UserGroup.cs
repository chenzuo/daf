using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Site.Models
{
    [Table("sys_UserGroup")]
    public class UserGroup : IOrdered
    {
        public UserGroup()
        {
            Parent = null;
            Children = new List<UserGroup>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Caption { get; set; }
        public int ShowOrder { get; set; }
        [StringLength(50)]
        public string ParentName { get; set; }

        [ForeignKey("SiteName, ParentName")]
        public virtual UserGroup Parent { get; set; }
        public virtual ICollection<UserGroup> Children { get; set; }
        public virtual ICollection<UserGroupUser> Users { get; set; }
        public virtual ICollection<CategoryUserGroup> AdminCategories { get; set; }
    }
}
