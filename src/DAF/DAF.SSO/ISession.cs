using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.SSO
{
    public interface ISession
    {
        string ClientId { get; set; }
        string ClientName { get; set; }
        string ClientBaseUrl { get; set; }
        string TransferedFromClientId { get; set; }

        string SessionId { get; set; }
        string AccessToken { get; set; }
        UserSession User { get; set; }
        string[] Roles { get; set; }
        SimplePermission[] Permissions { get; set; }

        string DeviceId { get; set; }
        string DeviceInfo { get; set; }

        string Theme { get; set; }
        string Skin { get; set; }
        string Locale { get; set; }
        double TimeZone { get; set; }
    }

    public static class ISessionExtensions
    {
        public static bool IsAuthenticated(this ISession session)
        {
            if (session == null || session.User == null)
                return false;
            return !string.IsNullOrEmpty(session.ClientId) && !string.IsNullOrEmpty(session.DeviceId) && !string.IsNullOrEmpty(session.SessionId) && !string.IsNullOrEmpty(session.User.UserId) && !string.IsNullOrEmpty(session.AccessToken);
        }

        public static bool CanAccess(this ISession session, string clientId, string protectedUri, PermissionType permissionType)
        {
            if (string.IsNullOrEmpty(protectedUri))
                return true;
            if (session == null)
                return false;
            if (session.Permissions == null || session.Permissions.Length <= 0)
                return true;

            var permission = session.Permissions.FirstOrDefault(o => o.ClientId == clientId && o.PermissionType == permissionType);
            if (permission == null)
                return true;

            protectedUri = protectedUri.ToLower();
            if (permission.ProtectedUris.Any(o => o == protectedUri))
            {
                return permission.AllowedUris.Any(o => o == protectedUri);
            }

            return true;
        }
    }
}
