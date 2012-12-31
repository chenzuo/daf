using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Site.Models
{
    [Table("sys_Category")]
    public class Category : IOrdered
    {
        public Category()
        {
            Parent = null;
            Children = new List<Category>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(10)]
        public string Language { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public int ShowOrder { get; set; }

        public DataStatus Status { get; set; }
        [StringLength(50)]
        public string ParentCode { get; set; }

        [ForeignKey("SiteName, Language, ParentCode")]
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<CategoryUserGroup> AdminUserGroups { get; set; }
    }
}