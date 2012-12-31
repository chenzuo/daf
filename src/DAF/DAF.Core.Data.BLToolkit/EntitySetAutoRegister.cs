using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core;

namespace DAF.Core.Data.BLToolkit
{
    public class EntitySetAutoRegister : IAutoRegisterContainerWithType
    {
        public void Register(ContainerBuilder builder, Type type)
        {
            if (type.IsClass && !type.IsAbstract && typeof(IEntitySet).IsAssignableFrom(type))
            {
                builder.RegisterType(type).As(typeof(IEntitySet)).Named(type.FullName, typeof(IEntitySet));
            }
        }
    }
}
