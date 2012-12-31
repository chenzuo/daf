using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core
{
    public interface IServiceFactory<T>
    {
        T Create(Type type, IComponentContext container);
    }
}
