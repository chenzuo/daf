using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using System.IO;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using DAF.Core;
using DAF.Core.FileSystem;

namespace DAF.Web.Mvc
{
    public class WebMvcGlobal : WebGlobal
    {
        protected override void BuildContainer(ContainerBuilder builder)
        {
            Config.Current.AssembiesToScan.ForEach(asm =>
                {
                    builder.RegisterControllers(asm);
                });
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            AreaRegistration.RegisterAllAreas();
        }
    }
}
