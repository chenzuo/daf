using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.CMS.Site.Models
{
    [Table("sys_CategoryUserGroup")]
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
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(10)]
        public string Language { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string CategoryCode { get; set; }

        [Required]
        [Key, Column(Order = 3)]
        [StringLength(50)]
        public string UserGroupName { get; set; }

        [ForeignKey("SiteName, Language, CategoryCode")]
        public virtual Category Category { get; set; }
        [ForeignKey("SiteName, UserGroupName")]
        public virtual UserGroup UserGroup { get; set; }
    }
}