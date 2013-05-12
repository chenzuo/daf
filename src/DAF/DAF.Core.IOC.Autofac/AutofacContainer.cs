using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;

namespace DAF.Core.IOC.Autofac
{
    public class AutofacContainer : AutofacContext, IIocContainer
    {
        protected IContainer container;

        public AutofacContainer(IContainer container)
            : base(container)
        {
            this.container = container;
        }

        public override bool IsRegistered(Type type, string name = null)
        {
            if (WorkUnitScope != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return WorkUnitScope.IsRegistered(type);
                }
                else
                {
                    return WorkUnitScope.IsRegisteredWithName(name, type);
                }
            }
            return base.IsRegistered(type, name);
        }

        public override object Resolve(Type type, string name = null)
        {
            if (WorkUnitScope != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    if(WorkUnitScope.IsRegistered(type))
                        return WorkUnitScope.Resolve(type);
                }
                else
                {
                    if(WorkUnitScope.IsRegisteredWithName(name, type))
                        return WorkUnitScope.ResolveNamed(name, type);
                }
            }
            return base.Resolve(type, name);
        }

        public override object ResolveOptional(Type type, string name = null)
        {
            if (WorkUnitScope != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    if (WorkUnitScope.IsRegistered(type))
                        return WorkUnitScope.ResolveOptional(type);
                }
                else
                {
                    if (WorkUnitScope.IsRegisteredWithName(name, type))
                        return WorkUnitScope.ResolveNamed(name, type);
                }
            }
            return base.Resolve(type, name);
        }

        public override IEnumerable<object> ResolveAll(Type type)
        {
            if (WorkUnitScope != null)
            {
                var stype = typeof(IEnumerable<>).MakeGenericType(type);
                return (IEnumerable<object>)WorkUnitScope.Resolve(stype);
            }
            return base.ResolveAll(type);
        }

        public virtual void BeginWorkUnitScope()
        {
            WorkUnitScope = container.BeginLifetimeScope("workunit");
        }

        public virtual void EndWorkUnitScope()
        {
            if (WorkUnitScope != null)
            {
                WorkUnitScope.Dispose();
                WorkUnitScope = null;
            }
        }

        public virtual ILifetimeScope WorkUnitScope
        {
            get { return HttpContext.Current.Items[typeof(ILifetimeScope)] as ILifetimeScope; }
            set { HttpContext.Current.Items[typeof(ILifetimeScope)] = value; }
        }

        public IContainer Container { get { return container; } }
    }
}
