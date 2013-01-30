using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace DAF.Web.Api.Filters
{
    public class ShareActionContextFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (ShareActionContext.Current == null)
                ShareActionContext.Current = actionContext;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ShareActionContext.Current = null;
        }
    }
}
