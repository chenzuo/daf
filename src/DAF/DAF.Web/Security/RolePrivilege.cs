using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core.Security;
using DAF.Web.Host;

namespace DAF.Web.Security
{
    [Table("daf_RolePrivilege")]
    public class RolePrivilege
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string AppName { get; set; }
        [Required]
        [MaxLength]
        public string Permissions { get; set; }
        public FieldScope FieldScope { get; set; }
        public DataScope DataScope { get; set; }

        [Required]
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
