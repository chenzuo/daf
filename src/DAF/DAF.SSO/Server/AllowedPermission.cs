using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;

namespace DAF.SSO.Server
{
    public class AllowedPermission
    {
        public Permission Permission { get; set; }
        public bool IsAllowed { get; set; }
    }

    public static class AllowedPermissionExtensions
    {
        public static string Format(this IEnumerable<Permission> permissions, IEnumerable<AllowedPermission> allowedPermissions)
        {
            if (permissions == null || permissions.Count() <= 0)
                return string.Empty;
            int length = permissions.Select(o => o.Position).Max() + 1;
            if (allowedPermissions == null || allowedPermissions.Count() <= 0)
            {
                return "0".Repeat(length);
            }
            char[] ps = new char[length];
            foreach (var p in permissions)
            {
                var ap = allowedPermissions.FirstOrDefault(o => o.Permission.Uri.ToLower() == p.Uri.ToLower() && o.Permission.Position == p.Position);
                ps[p.Position] = (ap != null && ap.IsAllowed) ? '1' : '0';
            }
            return new string(ps);
        }

        public static IEnumerable<AllowedPermission> Parse(this IEnumerable<Permission> permissions, string allowedPermissions)
        {
            foreach (var p in permissions)
            {
                yield return new AllowedPermission()
                {
                    Permission = p,
                    IsAllowed = !string.IsNullOrEmpty(allowedPermissions) && allowedPermissions.Length > p.Position && allowedPermissions[p.Position] == '1'
                };
            }
        }

        public static string[] Uris(this IEnumerable<Permission> permissions)
        {
            if (permissions == null || permissions.Count() <= 0)
                return new string[0];
            return permissions.Select(o => o.Uri).ToArray();
        }

        public static string[] Uris(this IEnumerable<AllowedPermission> allowedPermissions)
        {
            if (allowedPermissions == null || allowedPermissions.Count() <= 0)
                return new string[0];
            return allowedPermissions.Select(o => o.Permission.Uri).ToArray();
        }
    }
}
