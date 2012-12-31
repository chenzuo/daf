using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Configuration;
using DAF.Core;
using DAF.Core.Logging;
using DAF.Core.Localization;
using DAF.Web.Mvc.Results;

namespace DAF.Web.Mvc.Controllers
{
    [HandleError]
    public abstract class CommonController : System.Web.Mvc.Controller
    {
        public CommonController()
        {
            this.Localizer = NullLocalizer.Instance;
            this.Logger = NullLogger.Instance;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            
            if (requestContext.HttpContext.Session["ApplicationMessage"] != null)
            {
                ViewBag.Message = requestContext.HttpContext.Session["ApplicationMessage"];
                requestContext.HttpContext.Session.Remove("ApplicationMessage");
            }

            Type controllerType = this.GetType();
            string asmName = controllerType.Assembly.GetName().Name;
            ViewBag.Title = Localizer.Get(string.Format("{0}_{1}_Title", requestContext.RouteData.Values["controller"], requestContext.RouteData.Values["action"]), asmName);
            ViewBag.Description = Localizer.Get(string.Format("{0}_{1}_Description", requestContext.RouteData.Values["controller"], requestContext.RouteData.Values["action"]), asmName);
        }

        protected override RedirectResult Redirect(string url)
        {
            EnsureMessages();
            return base.Redirect(url);
        }

        protected override RedirectResult RedirectPermanent(string url)
        {
            EnsureMessages();
            return base.RedirectPermanent(url);
        }

        protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            EnsureMessages();
            return base.RedirectToAction(actionName, controllerName, routeValues);
        }

        protected override RedirectToRouteResult RedirectToActionPermanent(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            EnsureMessages();
            return base.RedirectToActionPermanent(actionName, controllerName, routeValues);
        }

        protected override RedirectToRouteResult RedirectToRoute(string routeName, RouteValueDictionary routeValues)
        {
            EnsureMessages();
            return base.RedirectToRoute(routeName, routeValues);
        }

        protected override RedirectToRouteResult RedirectToRoutePermanent(string routeName, RouteValueDictionary routeValues)
        {
            EnsureMessages();
            return base.RedirectToRoutePermanent(routeName, routeValues);
        }

        [NonAction]
        protected virtual ActionResult SmartView(string viewName, object model = null, string title = null, string msg = null, Func<string, object, ActionResult> getResult = null)
        {
            if (!string.IsNullOrEmpty(title))
                ViewBag.Title = title;
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Message = msg;
                Session["ApplicationMessage"] = msg;
            }

            if(getResult == null)
                return View(viewName, model);

            return getResult(viewName, model);
        }

        protected virtual void EnsureMessages()
        {
            object message = null;
            if (ViewBag.Message != null)
                message = ViewBag.Message;
            if (TempData["Message"] != null)
                message = TempData["Message"];

            if (message != null)
                this.Session["ApplicationMessage"] = message.ToString().Replace("'", "\"").Replace(Environment.NewLine, "<br/>");
        }

        public ILocalizer Localizer { get; set; }
        public ILogger Logger { get; set; }
    }
}
