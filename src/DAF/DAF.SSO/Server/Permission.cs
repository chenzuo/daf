using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.SSO.Server
{
    [Table("sso_Permission")]
    public class Permission
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string ClientId { get; set; }
        private string permissionName;
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string PermissionName { get { return permissionName; } set { permissionName = value.ToLower(); } }
        [Required]
        [Key, Column(Order = 2)]
        public PermissionType PermissionType { get; set; }

        public int Position { get; set; }

        private string uri;
        [Required]
        [StringLength(200)]
        public string Uri 
        {
            get { return uri; }
            set { uri = (value ?? "").ToLower(); }
        }

        [StringLength(50)]
        public string GroupName { get; set; }
    }
}
