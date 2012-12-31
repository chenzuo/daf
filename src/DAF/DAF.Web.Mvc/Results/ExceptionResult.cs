using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DAF.Web.Mvc.Results
{
    public class ExceptionResult : ViewResult
    {
        private Exception ex;

        public ExceptionResult(Exception ex)
        {
            this.ex = ex;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ViewName = "Error";

            ViewData = context.Controller.ViewData;
            TempData = context.Controller.TempData;

            string controller = context.RouteData.GetRequiredString("controller");
            string action = context.RouteData.GetRequiredString("action");

            HandleErrorInfo error = new HandleErrorInfo(ex, controller, action);

            ViewData.Model = error;

            base.ExecuteResult(context);
        }
    }
}
