using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_UserGroup")]
    public class UserGroup : IOrdered
    {
        public UserGroup()
        {
            Parent = null;
            Children = new List<UserGroup>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string UserGroupId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }
        public int ShowOrder { get; set; }
        [StringLength(50)]
        public string ParentId { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("UserGroupId")]
        public virtual UserGroup Parent { get; set; }
        public virtual ICollection<UserGroup> Children { get; set; }
        public virtual ICollection<UserGroupUser> Users { get; set; }
        public virtual ICollection<CategoryUserGroup> Categories { get; set; }
    }
}
