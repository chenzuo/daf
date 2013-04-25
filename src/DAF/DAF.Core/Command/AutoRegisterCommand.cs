using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;

namespace DAF.Core.Command
{
    public class AutoRegisterCommand : IAutoRegister
    {
        public void Register(IIocBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass && typeof(ICommand).IsAssignableFrom(type))
            {
                builder.RegisterType(typeof(ICommand), type, LiftTimeScope.Transiant, type.FullName);
            }
        }
    }
}
