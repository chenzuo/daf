using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Security;

namespace DAF.SSO.Server
{
    [Table("sso_RolePermission")]
    public class RolePermission
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ClientId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public PermissionType PermissionType { get; set; }
        [Required]
        [MaxLength]
        public string Permissions { get; set; }

        [Required]
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }

    public static class RolePermissionExtension
    {
        public static bool HasPermitted(this IEnumerable<RolePermission> rps, Permission permission)
        {
            if (permission == null)
                return true;
            if (rps == null || rps.Count() <= 0)
                return false;
            var rp = rps.FirstOrDefault(o => o.ClientId == permission.ClientId && o.PermissionType == permission.PermissionType);
            if (rp == null)
                return true;
            if (string.IsNullOrEmpty(rp.Permissions) || rp.Permissions.Length <= permission.Position)
                return false;
            return rp.Permissions[permission.Position] == '1';
        }

        public static string BuildPermissionString(this IEnumerable<Permission> permissions, IEnumerable<int> permittedPositions)
        {
            if (permissions == null || permissions.Count() <= 0)
                return string.Empty;
            int maxIdx = permissions.Select(o => o.Position).Max();
            char[] ps = new char[maxIdx + 1];
            if (permittedPositions == null || permittedPositions.Count() <= 0)
            {
                for(int i=0; i<maxIdx; i++)
                    ps[i] = '0';
            }
            foreach (var pos in permittedPositions)
            {
                if (permittedPositions != null && permittedPositions.Any(o => o == pos))
                    ps[pos] = '1';
                else
                    ps[pos] = '0';
            }
            return new string(ps);
        }
    }
}
