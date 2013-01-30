using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Models
{
    [Table("cms_CategoryUserGroup")]
    public class CategoryUserGroup
    {
        public CategoryUserGroup()
        {
            Category = null;
            UserGroup = null;
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string CategoryId { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string UserGroupId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("UserGroupId")]
        public virtual UserGroup UserGroup { get; set; }
    }
}