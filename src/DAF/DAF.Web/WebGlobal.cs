using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Web
{
    public abstract class WebGlobal : System.Web.HttpApplication
    {
        protected string iocConfigFile = "ioc.config";
        protected string[] IgnoreAssemblyFiles = new string[] { "system", "autofac", "bltoolkit", "entityframework", "microsoft", "newtonsoft", "nservicebus", "log4net", "emitmapper" };

        protected abstract IIocBuilder CreateIocBuilder();
        protected virtual void BuildeIOC(IIocBuilder builder)
        {
            var file = ("~/Configurations/" + iocConfigFile).GetPhysicalPath();
            builder.RegisterConfig(file);
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Config.Current.IgnoreAssemblies(IgnoreAssemblyFiles).With();
            IIocBuilder builder = CreateIocBuilder();

            IocInstance.RegisterBuilder(builder);
            IocInstance.AutoRegister(Config.Current.TypesToScan);
            BuildeIOC(builder);
            IocInstance.Build();

            IocInstance.Start(this.Context);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            IocInstance.Stop(this.Context);
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom == "session")
            {
                return context.Request["ASP.NET_SessionId"];
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}
