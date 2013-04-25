using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;

namespace DAF.Core.IOC.Autofac
{
    public class AutofacContext : IIocContext
    {
        private IComponentContext context;

        public AutofacContext(IComponentContext context)
        {
            this.context = context;
        }

        public bool IsRegistered<T>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.IsRegistered<T>();
            }
            else
            {
                return context.IsRegisteredWithName<T>(name);
            }
        }

        public T Resolve<T>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.Resolve<T>();
            }
            else
            {
                return context.ResolveNamed<T>(name);
            }
        }

        public T ResolveOptional<T>(string name = null)
            where T : class
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.ResolveOptional<T>();
            }
            else
            {
                return context.ResolveOptionalNamed<T>(name);
            }
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return context.Resolve<IEnumerable<T>>();
        }
    }
}
