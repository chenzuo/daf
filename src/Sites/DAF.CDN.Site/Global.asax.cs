using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using DAF.Core.IOC;

namespace DAF.CDN.Site
{
    public class Global : DAF.Web.WebGlobal
    {
        protected override IIocBuilder CreateIocBuilder()
        {
            return new DAF.Core.IOC.Autofac.AutofacBuilder();
        }
    }
}