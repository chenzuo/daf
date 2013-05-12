using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
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

        public virtual bool IsRegistered(Type type, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.IsRegistered(type);
            }
            else
            {
                return context.IsRegisteredWithName(name, type);
            }
        }

        public virtual object Resolve(Type type, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.Resolve(type);
            }
            else
            {
                return context.ResolveNamed(name, type);
            }
        }

        public virtual object ResolveOptional(Type type, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return context.ResolveOptional(type);
            }
            else
            {
                object obj = null;
                context.TryResolveNamed(name, type, out obj);
                return obj;
            }
        }

        public virtual IEnumerable<object> ResolveAll(Type type)
        {
            var stype = typeof(IEnumerable<>).MakeGenericType(type);
            return (IEnumerable<object>)context.Resolve(stype);
        }
    }
}
