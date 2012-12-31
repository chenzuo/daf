using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core.Localization;
using DAF.Core.Logging;

namespace DAF.Core
{
    public class IOC
    {
        private static object @lock = new object();
        private static IContainer global_container;

        public static IContainer Current
        {
            get
            {
                return global_container;
            }
        }

        public static IContainer SetContainer(IContainer container)
        {
            lock (@lock)
            {
                global_container = container;
            }
            return container;
        }
    }

    public static class IOCExtensions
    {
        public static object GetService(this IContainer container, Type serviceType, object defaultService = null)
        {
            var svr = container.ResolveOptional(serviceType);
            if (svr == null)
                return defaultService;
            return svr;
        }

        public static IService GetService<IService>(this IContainer container, IService defaultService = default(IService))
        {
            var svr = (IService)container.ResolveOptional(typeof(IService));
            if (svr == null)
                return defaultService;
            return svr;
        }
    }
}
