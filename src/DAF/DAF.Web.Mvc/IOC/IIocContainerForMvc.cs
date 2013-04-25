using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAF.Core.IOC;

namespace DAF.Web.Mvc.IOC
{
    public interface IIocContainerForMvc : IIocContainer
    {
        IDependencyResolver GetDependencyResolver();
    }
}
