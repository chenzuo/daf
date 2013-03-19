using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.Command
{
    public class AutoRegisterCommand : IAutoRegisterContainerWithType
    {
        public void Register(ContainerBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass && typeof(ICommand).IsAssignableFrom(type))
            {
                builder.RegisterType(type).As(typeof(ICommand)).Named(type.FullName, typeof(ICommand));
            }
        }
    }
}
