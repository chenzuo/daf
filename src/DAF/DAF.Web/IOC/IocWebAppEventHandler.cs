using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;

namespace DAF.Web.IOC
{
    public class IocWebAppEventHandler : IWebAppEventHandler
    {
        public void Application_Start(object sender, EventArgs e)
        {
        }

        public void Application_End(object sender, EventArgs e)
        {
        }

        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            IocInstance.Container.BeginWorkUnitScope();
        }

        public void Application_EndRequest(object sender, EventArgs e)
        {
            IocInstance.Container.EndWorkUnitScope();
        }

        public void Application_Error(object sender, EventArgs e)
        {
        }

        public void Session_Start(object sender, EventArgs e)
        {
        }

        public void Session_End(object sender, EventArgs e)
        {
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Earliest; }
        }
    }
}
