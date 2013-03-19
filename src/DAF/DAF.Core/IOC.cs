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

        public static void Start(this IContainer container, object context = null)
        {
            var appEventHandlers = container.ResolveOptional<IEnumerable<IAppEventHandler>>();
            if (appEventHandlers != null)
            {
                foreach (var aeh in appEventHandlers.OrderBy(o => o.ExecuteOrder))
                    aeh.OnApplicationStart(container, context);
            }

            var startups = container.ResolveOptional<IEnumerable<IStartup>>();
            if (startups != null)
            {
                foreach (var aeh in startups.OrderBy(o => o.ExecuteOrder))
                    aeh.OnStarted();
            }
        }

        public static void Stop(this IContainer container, object context = null)
        {
            var appEventHandlers = container.ResolveOptional<IEnumerable<IAppEventHandler>>();
            if (appEventHandlers != null)
            {
                foreach (var aeh in appEventHandlers.OrderBy(o => o.ExecuteOrder))
                    aeh.OnApplicatoinExit(container, context);
            }
        }
    }
}
