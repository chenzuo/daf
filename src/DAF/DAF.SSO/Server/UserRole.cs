using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.SSO.Server
{
    [Table("sso_UserRole")]
    public class UserRole
    {
        [Key, Column(Order = 0)]
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string RoleId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public virtual User User { get; set; }
        [ForeignKey("RoleId")]
        [Required]
        public virtual Role Role { get; set; }
    }
}
