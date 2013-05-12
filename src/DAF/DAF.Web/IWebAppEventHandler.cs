using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DAF.Web
{
    public interface IWebAppEventHandler
    {
        void Application_Start(object sender, EventArgs e);
        void Application_End(object sender, EventArgs e);
        void Application_AuthenticateRequest(object sender, EventArgs e);
        void Application_BeginRequest(object sender, EventArgs e);
        void Application_EndRequest(object sender, EventArgs e);
        void Application_Error(object sender, EventArgs e);
        void Session_Start(object sender, EventArgs e);
        void Session_End(object sender, EventArgs e);
        int ExecuteOrder { get; }
    }
}
