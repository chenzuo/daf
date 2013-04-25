using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using DAF.Core.IOC;
using DAF.Web.Mvc.IOC;
using DAF.Core.IOC.Autofac;

namespace DAF.Core.IOC.AutofacForMvc
{
    public class AutofacBuilderForMvc : AutofacBuilder, IIocBuilderForMvc
    {
        public void RegisterControllers(Assembly asm)
        {
            builder.RegisterControllers(asm);
        }

        public override IIocContainer Build()
        {
            builder.RegisterModule(module);
            var container = builder.Build();
            return new AutofacContainerForMvc(container);
        }

    }
}
