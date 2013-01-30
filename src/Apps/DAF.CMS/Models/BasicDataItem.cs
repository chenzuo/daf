using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DAF.CMS.Models
{
    [Table("cms_BasicData")]
    public class BasicDataItem : DAF.Core.IOrdered
    {
        public BasicDataItem()
        {
            Parent = null;
            Children = new List<BasicDataItem>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string ItemId { get; set; }
        [Required]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
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
        public string ParentId { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        [ForeignKey("ParentId")]
        public virtual BasicDataItem Parent { get; set; }
        public virtual ICollection<BasicDataItem> Children { get; set; }
    }
}