using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Core.Data.BLToolkit
{
    public class EntitySetAutoRegister : IAutoRegister
    {
        public void Register(IIocBuilder builder, Type type)
        {
            if (type.IsClass && !type.IsAbstract && typeof(IEntitySet).IsAssignableFrom(type))
            {
                builder.RegisterType(typeof(IEntitySet), type, LiftTimeScope.Transiant, type.FullName);
            }
        }
    }
}
