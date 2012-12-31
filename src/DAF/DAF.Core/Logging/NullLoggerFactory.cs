using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.Logging
{
    public class NullLoggerFactory : IServiceFactory<ILogger>
    {
        public ILogger Create(Type type, IComponentContext container)
        {
            return NullLogger.Instance;
        }
    }
}
