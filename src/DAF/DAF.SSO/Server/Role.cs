using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;

namespace DAF.SSO.Server
{
    [Table("sso_Role")]
    public class Role : IEntityWithStatus
    {
        public Role()
        {
            Parent = null;
            Children = new List<Role>();
            Privileges = new List<RolePermission>();
            RoleUsers = new List<UserRole>();
        }

        [Required]
        [Key]
        [StringLength(50)]
        public string RoleId { get; set; }

        [StringLength(50)]
        public string ClientId { get; set; }
        private string roleName;
        [Required]
        [StringLength(50)]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value.ToLower(); }
        }

        public DateTime? ActiveTime { get; set; }
        public DateTime? ExpiryTime { get; set; }

        public DataStatus Status { get; set; }
        [StringLength(50)]
        public string ParentRoleId { get; set; }

        [ForeignKey("ParentRoleId")]
        public virtual Role Parent { get; set; }
        public virtual ICollection<Role> Children { get; set; }
        public virtual ICollection<RolePermission> Privileges { get; set; }
        public virtual ICollection<UserRole> RoleUsers { get; set; }
    }
}
