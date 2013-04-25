using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using DAF.Core.IOC;
using DAF.Web.Api;

namespace DAF.CMS.Site
{
    public class Global : CmsWebGlobal
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
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "category/{cate3}/{cate2}/{cate1}",
                defaults: null,
                constraints: null,
                handler: new CmsMessageHandler("category")
                );
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "CategoryPage2",
                routeTemplate: "{cate2}/{cate1}/{pid}",
                defaults: null,
                constraints: null,
                handler: new CmsMessageHandler()
                );
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "CategoryPage",
                routeTemplate: "{cate1}/{pid}",
                defaults: null,
                constraints: null,
                handler: new CmsMessageHandler()
                );
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultPage",
                routeTemplate: "{pid}",
                defaults: null,
                constraints: null,
                handler: new CmsMessageHandler()
                );
        }

        protected override IIocBuilder CreateIocBuilder()
        {
            return new DAF.Core.IOC.AutofacForApi.AutofacBuilderForApi();
        }
    }
}