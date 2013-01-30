using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using DAF.Core;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.Web;
using DAF.Web.Security;

namespace DAF.Web.Api.Filters
{
    public class OAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public OAuthorizeAttribute()
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
             || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                return;

            var uri = actionContext.Request.RequestUri;
            if (!AuthHelper.IsAuthenticated)
            {
                AuthHelper.AutoSignOn(
                    () =>
                    {
                        string authInfo = null;
                        if (actionContext.Request.Headers.Authorization != null)
                            authInfo = actionContext.Request.Headers.Authorization.Parameter;

                        if (string.IsNullOrEmpty(authInfo))
                        {
                            var sessionCookie = actionContext.Request.Headers.GetCookies().FirstOrDefault().Cookies.FirstOrDefault(o => o.Name == "sid");
                            if (sessionCookie != null)
                                authInfo = sessionCookie.Value.Replace(" ", "+");
                        }
                        return authInfo;
                    },
                    () =>
                    {
                        TransferSignOnInfo tso = null;
                        var fromUri = actionContext.Request.Headers.Referrer;
                        if (fromUri != null && fromUri.BaseUrl() != uri.BaseUrl())
                        {
                            var cookies = actionContext.Request.Headers.GetCookies();
                            var queryString = actionContext.Request.GetQueryNameValuePairs();
                            var fc = queryString.FirstOrDefault(o => o.Key == "fcid");
                            var sid = queryString.FirstOrDefault(o => o.Key == "sid");
                            tso = new TransferSignOnInfo()
                            {
                                ClientId = AuthHelper.CurrentClient.ClientId,
                                DeviceId = actionContext.Request.Headers.Host,
                                DeviceInfo = actionContext.Request.Headers.UserAgent.First().Product.Name,
                                SessionId = Thread.CurrentThread.ManagedThreadId.ToString(),
                                FromClientId = fc.Value,
                                FromSessionId = sid.Value
                            };
                        }
                        return tso;
                    });
            }

            if (AuthHelper.IsAuthenticated)
            {
                // 用户已经登录，判断权限
                if (AuthHelper.CurrentSession.CanAccess(AuthHelper.CurrentClient.ClientId, uri.AbsoluteUri, PermissionType.Operation))
                {
                    return;
                }
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "");
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
                actionContext.Response.Headers.Location = new Uri("/Account/SignOn", UriKind.Relative);
            }
        }
    }
}
