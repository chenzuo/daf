using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core
{
    public class AutoRegisterStartup : IAutoRegisterContainerWithType
    {
        public void Register(ContainerBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass && typeof(IStartup).IsAssignableFrom(type))
            {
                builder.RegisterType(type).As(typeof(IStartup)).Named(type.FullName, typeof(IStartup));
            }
        }
    }
}
