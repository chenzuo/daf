using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DAF.Core.IOC;

namespace DAF.Web.Mvc.IOC
{
    public interface IIocBuilderForMvc : IIocBuilder
    {
        void RegisterControllers(Assembly asm);
    }
}
