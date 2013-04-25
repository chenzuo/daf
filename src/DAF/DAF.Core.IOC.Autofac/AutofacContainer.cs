using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.IOC.Autofac
{
    public class AutofacContainer : IIocContainer
    {
        protected IContainer container;

        public AutofacContainer(IContainer container)
        {
            this.container = container;
        }

        public void BeginWorkUnitScope()
        {
            container.BeginLifetimeScope("workunit");
        }

        public bool IsRegistered<T>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.IsRegistered<T>();
            }
            else
            {
                return container.IsRegisteredWithName<T>(name);
            }
        }

        public T Resolve<T>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.Resolve<T>();
            }
            else
            {
                return container.ResolveNamed<T>(name);
            }
        }

        public T ResolveOptional<T>(string name = null)
            where T : class
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.ResolveOptional<T>();
            }
            else
            {
                return container.ResolveOptionalNamed<T>(name);
            }
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return container.Resolve<IEnumerable<T>>();
        }

        public IContainer Container { get { return container; } }
    }
}
