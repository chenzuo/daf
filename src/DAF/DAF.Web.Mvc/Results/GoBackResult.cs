using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DAF.Web.Mvc.Results
{
    public class GoBackResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context.Controller.ViewBag.Message != null)
                context.HttpContext.Session["ApplicationMessage"] = context.Controller.ViewBag.Message;
            string url = "window.top.location";
            if (context.HttpContext.Request.UrlReferrer != null && !string.IsNullOrEmpty(context.HttpContext.Request.UrlReferrer.AbsolutePath))
                url = string.Format("'{0}'", context.HttpContext.Request.UrlReferrer.OriginalString);
            string scripts = string.Format("<script type='text/javascript'>window.top.location = {0};</script>", url);
            context.HttpContext.Response.Write(scripts);
            context.HttpContext.Response.End();
        }
    }
}
