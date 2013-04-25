using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.IOC
{
    public class IocInstance
    {
        private static IIocBuilder builder;
        private static IIocContainer container;

        public static void RegisterBuilder(IIocBuilder builder)
        {
            IocInstance.builder = builder;
        }

        public static void Build()
        {
            if (builder != null)
            {
                container = builder.Build();
            }
        }

        public static void AutoRegister(IEnumerable<Type> types)
        {
            List<IAutoRegister> autoRegisters = new List<IAutoRegister>();
            types.Where(t => t.IsClass && typeof(IAutoRegister).IsAssignableFrom(t))
               .ForEach(o =>
               {
                   if (o.GetConstructor(Type.EmptyTypes) != null)
                       autoRegisters.Add(Activator.CreateInstance(o) as IAutoRegister);
               });

            types.ForEach(t =>
                {
                    autoRegisters.ForEach(o => o.Register(builder, t));
                });
        }

        public static void Start(object context = null)
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

        public static void Stop(object context = null)
        {
            var appEventHandlers = container.ResolveOptional<IEnumerable<IAppEventHandler>>();
            if (appEventHandlers != null)
            {
                foreach (var aeh in appEventHandlers.OrderBy(o => o.ExecuteOrder))
                    aeh.OnApplicatoinExit(container, context);
            }
        }


        public static IIocBuilder Builder
        {
            get
            {
                return builder;
            }
        }

        public static IIocContainer Container
        {
            get
            {
                return container;
            }
        }
    }
}
