using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DAF.Core.IOC;

namespace DAF.Web.Api.IOC
{
    public interface IIocBuilderForApi : IIocBuilder
    {
        void RegisterApiControllers(Assembly asm);
    }
}
