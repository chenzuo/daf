using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Web.Security;

namespace DAF.CMS.Models
{
    [Table("cms_UserGroupUser")]
    public class UserGroupUser
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string UserGroupId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string UserId { get; set; }

        [ForeignKey("UserGroupId")]
        public virtual UserGroup UserGroup { get; set; }
    }
}