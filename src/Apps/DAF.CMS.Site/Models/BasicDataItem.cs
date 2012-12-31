using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DAF.CMS.Site.Models
{
    [Table("sys_BasicData")]
    public class BasicDataItem : DAF.Core.IOrdered
    {
        public BasicDataItem()
        {
            Parent = null;
            Children = new List<BasicDataItem>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteName { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ClientId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [Key, Column(Order = 3)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Caption { get; set; }
        [StringLength(50)]
        public string Value { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        public int ShowOrder { get; set; }
        public bool IsValid { get; set; }
        [StringLength(50)]
        public string ParentName { get; set; }

        [ForeignKey("SiteName, ClientId, Category, ParentName")]
        public virtual BasicDataItem Parent { get; set; }
        public virtual ICollection<BasicDataItem> Children { get; set; }
    }
}