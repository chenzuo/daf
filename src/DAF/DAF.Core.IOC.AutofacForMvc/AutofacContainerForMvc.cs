using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using DAF.Core.IOC;
using DAF.Web.Mvc.IOC;
using DAF.Core.IOC.Autofac;

namespace DAF.Core.IOC.AutofacForMvc
{
    public class AutofacContainerForMvc : AutofacContainer, IIocContainerForMvc
    {
        public AutofacContainerForMvc(IContainer container)
            : base(container)
        {
        }

        public IDependencyResolver GetDependencyResolver()
        {
            return new AutofacDependencyResolver(container);
        }
    }
}
