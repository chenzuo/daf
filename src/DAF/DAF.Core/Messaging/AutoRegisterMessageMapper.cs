using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.Messaging
{
    public class AutoRegisterMessageMapper : IAutoRegisterContainerWithType
    {
        public void Register(ContainerBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass)
            {
                var itypes = type.GetInterfaces();
                if (itypes != null && itypes.Length > 0)
                {
                    var itype = type.GetInterfaces()[0];
                    if (itype.IsGenericType && itype.GetGenericTypeDefinition().Equals(typeof(IMessageMapper<>)))
                    {
                        builder.RegisterType(type).As(itype).Named(type.FullName, itype);
                    }
                }
            }
        }
    }
}
