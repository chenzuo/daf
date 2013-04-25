using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.IOC
{
    public class AutoRegisterStartup : IAutoRegister
    {
        public void Register(IIocBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass && typeof(IStartup).IsAssignableFrom(type))
            {
                builder.RegisterType(typeof(IStartup), type, LiftTimeScope.Transiant, type.FullName);
            }
        }
    }
}
