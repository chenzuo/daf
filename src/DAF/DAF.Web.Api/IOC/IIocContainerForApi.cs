using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;
using DAF.Core.IOC;

namespace DAF.Web.Api.IOC
{
    public interface IIocContainerForApi : IIocContainer
    {
        IDependencyResolver GetDependencyResolver();
    }
}
