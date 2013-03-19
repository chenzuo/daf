using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Configuration;
using DAF.Core;

namespace DAF.Web
{
    public class WebGlobal : System.Web.HttpApplication
    {
        protected virtual void BuildContainer(ContainerBuilder builder)
        {
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Config.Current.InitializeDefaultIOC(BuildContainer);
            IOC.Current.Start(this.Context);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            IOC.Current.Stop(this.Context);
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
