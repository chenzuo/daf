using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using DAF.Core.IOC;
using DAF.Web.Api.IOC;
using DAF.Core.IOC.Autofac;

namespace DAF.Core.IOC.AutofacForApi
{
    public class AutofacContainerForApi : AutofacContainer, IIocContainerForApi
    {
        public AutofacContainerForApi(IContainer container)
            : base(container)
        {
        }

        public IDependencyResolver GetDependencyResolver()
        {
            return new AutofacWebApiDependencyResolver(container);
        }
    }
}
