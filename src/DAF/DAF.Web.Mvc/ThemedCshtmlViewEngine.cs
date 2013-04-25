using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Compilation;
using System.Web.Routing;
using System.Globalization;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Security;

namespace DAF.Web.Mvc
{
    public class ThemedCshtmlViewEngine : BuildManagerViewEngine
    {
        internal static readonly string ViewStartFileName;

        static ThemedCshtmlViewEngine()
        {
            ViewStartFileName = "_ViewStart";
        }

        public ThemedCshtmlViewEngine()
        {
            base.AreaViewLocationFormats = new string[] { 
                "~/Areas/{2}/Views/{1}/{0}.cshtml", 
                "~/Areas/{2}/Views/Shared/{0}.cshtml", 
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };
            base.AreaMasterLocationFormats = new string[] { 
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            base.AreaPartialViewLocationFormats = new string[]{
                "~/Areas/{2}/Views/{1}/{0}.cshtml", 
                "~/Areas/{2}/Views/Shared/{0}.cshtml", 
                "~/Views/{1}/{0}.cshtml",
                "~/Themes/{3}/{0}.cshtml",
                "~/Themes/Default/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };

            base.ViewLocationFormats = new string[] { 
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            base.MasterLocationFormats = new string[] { 
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            base.PartialViewLocationFormats = new string[]{
                "~/Views/{1}/{0}.cshtml",
                "~/Themes/{3}/{0}.cshtml",
                "~/Themes/Default/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            this.ViewStartFileExtensions = new string[] { "cshtml" };
        }

        protected override string GetFormatedVirtualPath(HttpContextBase context, VirtualPathProviderViewEngine.ViewLocation location, string name, string controllerName, string areaName)
        {
            var clientProvider = IocInstance.Container.Resolve<ISSOClientProvider>();
            var session = clientProvider == null ? null : clientProvider.GetCurrentSession();
            string theme = session == null ? "Default" : session.Theme;
            string skin = session == null ? "Default" : session.Skin;

            return string.Format(location.VirtualPathFormatString, name, controllerName, areaName, theme, skin);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            bool runViewStartPages = false;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, this.ViewStartFileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            bool runViewStartPages = true;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, this.ViewStartFileExtensions);
        }

        protected override bool IsValidCompiledType(ControllerContext controllerContext, string virtualPath, Type compiledType)
        {
            return typeof(WebViewPage).IsAssignableFrom(compiledType);
        }

        public string[] ViewStartFileExtensions { get; set; }
    }
}
