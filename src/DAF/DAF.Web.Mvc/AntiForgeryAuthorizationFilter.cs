using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DAF.Web.Mvc
{
    public class AntiForgeryAuthorizationFilter : IAuthorizationFilter, IMvcFilter
    {
        public AntiForgeryAuthorizationFilter()
        {
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!(filterContext.RouteData.Values["validateAntiForgeryToken"] is bool
                && (bool)filterContext.RouteData.Values["validateAntiForgeryToken"]
                && filterContext.HttpContext.Request.HttpMethod == "POST"
                && filterContext.RequestContext.HttpContext.Request.IsAuthenticated))
            {
                return;
            }

            string host = filterContext.HttpContext.Request.Url.DnsSafeHost;

            ValidateAntiForgeryTokenAttribute validator = new ValidateAntiForgeryTokenAttribute();

            validator.OnAuthorization(filterContext);
        }

        #endregion

        #region IMvcFilter Members

        public bool AllowMultiple
        {
            get { return true; }
        }

        public int Order
        {
            get { return (int)FilterScope.Global; }
        }

        #endregion
    }
}
