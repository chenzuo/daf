using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.IOC
{
    public interface IServiceFactory<T>
    {
        T Create(Type type, IIocContext context);
    }
}
