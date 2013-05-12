using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using DAF.Core.IOC;
using DAF.Web.Api.IOC;
using DAF.Core.IOC.Autofac;

namespace DAF.Core.IOC.AutofacForApi
{
    public class AutofacBuilderForApi : AutofacBuilder, IIocBuilderForApi
    {
        public void RegisterApiControllers(params Assembly[] asms)
        {
            builder.RegisterApiControllers(asms);
        }

        public override IIocContainer Build()
        {
            builder.RegisterModule(module);
            var container = builder.Build();
            return new AutofacContainerForApi(container);
        }
    }
}
