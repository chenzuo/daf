using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using System.IO;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.FileSystem;

namespace DAF.Web.Mvc
{
    public abstract class WebMvcGlobal : WebGlobal
    {
        protected override void BuildeIOC(IIocBuilder builder)
        {
            base.BuildeIOC(builder);
            
            if (builder is IOC.IIocBuilderForMvc)
            {
                ((IOC.IIocBuilderForMvc)builder).RegisterControllers(Config.Current.AssembiesToScan.ToArray());
            }
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            AreaRegistration.RegisterAllAreas();
        }
    }
}
