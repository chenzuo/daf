using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core
{
    public interface IAutoRegisterContainerWithType
    {
        void Register(ContainerBuilder builder, Type type);
    }
}
