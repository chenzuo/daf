using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net.Http;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Serialization;
using DAF.Core.Localization;
using DAF.SSO;
using DAF.SSO.Server;
using DAF.SSO.Client;
using DAF.Web.Mvc.Results;
using DAF.Web.Security;

namespace DAF.Web.Mvc
{
    public class OAuthorizeAttribute : FilterAttribute, IAuthorizationFilter, IMvcFilter
    {
        public OAuthorizeAttribute()
        {
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(true).Any(f => f is AllowAnonymousAttribute))
                return;

            if (!AuthHelper.IsAuthenticated)
            {
                var fromClientId = filterContext.HttpContext.Request.QueryString["fc"];
                if (!string.IsNullOrEmpty(fromClientId))
                {
                    var clientProvider = IOC.Current.GetService<ISSOClientProvider>();
                    var response = clientProvider.TransferSignOn(new TransferSignOnInfo()
                    {
                        ClientId = AuthHelper.CurrentClient.ClientId,
                        SessionId = AuthHelper.CurrentSession.SessionId,
                        FromClientId = fromClientId,
                        DeviceId = filterContext.HttpContext.Request.UserHostAddress,
                        DeviceInfo = filterContext.HttpContext.Request.UserAgent
                    });
                    if (response.Status == ResponseStatus.Success)
                    {
                        goto CheckAccessPermission;
                    }
                }
            }

            CheckAccessPermission:
            if (AuthHelper.IsAuthenticated)
            {
                // 用户已经登录，判断权限
                var permUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
                if (AuthHelper.CurrentSession.CanAccess(AuthHelper.CurrentClient.ClientId, permUrl, PermissionType.Operation))
                {
                    return;
                }
            }
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        #endregion
    }
}
