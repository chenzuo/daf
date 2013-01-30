using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_Category")]
    public class Category : IOrdered
    {
        public Category()
        {
            Parent = null;
            Children = new List<Category>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        public CategoryType CategoryType { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public int ShowOrder { get; set; }

        public DataStatus Status { get; set; }
        [StringLength(50)]
        public string ParentId { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<CategoryContent> Contents { get; set; }
        public virtual ICollection<CategoryUserGroup> UserGroups { get; set; }
    }
}