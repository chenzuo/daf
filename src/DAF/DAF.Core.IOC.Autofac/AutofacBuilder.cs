using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Builder;

namespace DAF.Core.IOC.Autofac
{
    public class AutofacBuilder : IIocBuilder
    {
        protected ContainerBuilder builder;
        protected AttachComponentModule module;

        public AutofacBuilder()
        {
            builder = new ContainerBuilder();
            module = new AttachComponentModule();
        }

        public void RegisterType(Type interfaceType, Type serviceType, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false,
            Func<IIocContext, IDictionary<string, object>> getConstructorParameters = null, Func<IIocContext, object, object> onActivating = null, Action<IIocContext, object> onActivated = null)
        {
            var rb = builder.RegisterType(serviceType).As(interfaceType);
            if (!string.IsNullOrEmpty(name))
                rb.Named(name, interfaceType);
            switch (scope)
            {
                case LifeTimeScope.Singleton:
                    rb.SingleInstance();
                    break;
                case LifeTimeScope.WorkUnit:
                    rb.InstancePerMatchingLifetimeScope("workunit");
                    break;
                case LifeTimeScope.Transient:
                default:
                    rb.InstancePerDependency();
                    break;
            }
            if (getConstructorParameters != null)
            {
                rb.OnPreparing(pe =>
                    {
                        var paras = getConstructorParameters(new AutofacContext(pe.Context));
                        if (paras != null)
                        {
                            pe.Parameters = paras.Select(kv => new NamedParameter(kv.Key, kv.Value));
                        }
                    });
            }
            if (onActivating != null)
            {
                rb.OnActivating(ae =>
                    {
                        var obj = onActivating(new AutofacContext(ae.Context), ae.Instance);
                        ae.ReplaceInstance(obj);
                    });
            }
            if (onActivated != null)
            {
                rb.OnActivated(ae =>
                {
                    onActivated(new AutofacContext(ae.Context), ae.Instance);
                });
            }

            if (autoWire)
            {
                module.AddAutoWiredType(interfaceType);
            }
        }

        public void RegisterInstance(Type interfaceType, object instance, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false)
        {
            var rb = builder.RegisterInstance(instance).As(interfaceType);
            if (!string.IsNullOrEmpty(name))
                rb.Named(name, interfaceType);
            switch (scope)
            {
                case LifeTimeScope.Singleton:
                    rb.SingleInstance();
                    break;
                case LifeTimeScope.WorkUnit:
                    rb.InstancePerMatchingLifetimeScope("workunit");
                    break;
                case LifeTimeScope.Transient:
                default:
                    rb.InstancePerDependency();
                    break;
            }

            if (autoWire)
            {
                module.AddAutoWiredType(interfaceType);
            }
        }

        public void RegisterFactory<IT, TT>(LifeTimeScope scope = LifeTimeScope.Transient)
            where TT : class, IServiceFactory<IT>
        {
            builder.RegisterType<TT>().As<IServiceFactory<IT>>().SingleInstance();

            var rb = builder.Register((c, p) =>
            {
                if (p.Count() <= 0)
                    throw new Exception("cannot get the parameters from the constructor");

                Type containerType = p.TypedAs<Type>();

                if(containerType.IsGenericType)
                    containerType = containerType.GetGenericArguments()[0];
                var fac = c.Resolve<IServiceFactory<IT>>();
                return fac.Create(containerType, new AutofacContext(c));
            }).As<IT>();

            module.AddTypeBasedServiceType(typeof(IT));
        }

        public void RegisterGeneric(Type interfaceType, Type serviceType, LifeTimeScope scope = LifeTimeScope.Transient)
        {
            var rb = builder.RegisterGeneric(serviceType).As(interfaceType);
            switch (scope)
            {
                case LifeTimeScope.Singleton:
                    rb.SingleInstance();
                    break;
                case LifeTimeScope.WorkUnit:
                    rb.InstancePerMatchingLifetimeScope("workunit");
                    break;
                case LifeTimeScope.Transient:
                default:
                    rb.InstancePerDependency();
                    break;
            }
        }

        public virtual IIocContainer Build()
        {
            builder.RegisterModule(module);
            var container = builder.Build();
            return new AutofacContainer(container);
        }
    }
}
