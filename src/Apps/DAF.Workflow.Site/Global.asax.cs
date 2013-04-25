using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using DAF.Core.IOC;
using DAF.Web.Api;

namespace DAF.Workflow.Site
{
    public class Global : DAF.Web.Api.WebApiGlobal
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultActionApi",
                routeTemplate: "api/{controller}/{action}"
                );

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
                );
        }

        protected override IIocBuilder CreateIocBuilder()
        {
            return new DAF.Core.IOC.AutofacForApi.AutofacBuilderForApi();
        }
    }
}