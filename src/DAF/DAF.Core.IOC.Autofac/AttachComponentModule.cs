using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Builder;
using Module = Autofac.Module;

namespace DAF.Core.IOC.Autofac
{
    public class AttachComponentModule : Module
    {
        private List<Type> autoWireTypes;
        private List<Type> typebasedServiceTypes;

        public AttachComponentModule()
        {
            autoWireTypes = new List<Type>();
            typebasedServiceTypes = new List<Type>();
        }

        public void AddAutoWiredType(Type autoWireType)
        {
            autoWireTypes.Add(autoWireType);
        }

        public void AddTypeBasedServiceType(Type serviceType)
        {
            typebasedServiceTypes.Add(serviceType);
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var implementationType = registration.Activator.LimitType;

            foreach (var autoWireType in autoWireTypes)
            {
                var constructors = implementationType.GetConstructorsWithDependency(autoWireType);

                if (constructors.Any())
                {
                    registration.Preparing += (sender, e) =>
                    {
                        var parameter = new TypedParameter(autoWireType,
                            e.Context.Resolve(autoWireType, new TypedParameter(typeof(Type), implementationType)));
                        e.Parameters = e.Parameters.Concat(new[] { parameter });
                    };
                }
                else
                {
                    var props = implementationType.GetPropertiesWithDependency(autoWireType);
                    if (props.Any())
                    {
                        registration.Activated += (s, e) =>
                        {
                            foreach (var prop in props)
                            {
                                prop.SetValue(e.Instance,
                                    e.Context.Resolve(autoWireType));
                            }
                        };
                    }
                }
            }

            foreach (var serviceType in typebasedServiceTypes)
            {
                var constructorInjectors = BuildConstructorServiceInjectors(implementationType, serviceType).ToArray();
                if (constructorInjectors.Any())
                {
                    registration.Preparing += (s, e) =>
                    {
                        foreach (var ci in constructorInjectors)
                            ci(e);
                    };
                    return;
                }

                // build an array of actions on this type to assign loggers to member properties
                var injectors = BuildPropertyServiceInjectors(implementationType, serviceType).ToArray();

                if (injectors.Any())
                {
                    registration.Activated += (s, e) =>
                    {
                        foreach (var injector in injectors)
                            injector(e.Context, e.Instance);
                    };
                }
            }
        }

        private IEnumerable<Action<PreparingEventArgs>> BuildConstructorServiceInjectors(Type componentType, Type serviceType)
        {
            var constructors = componentType.GetConstructorsWithDependency(serviceType);
            foreach (var constructInfo in constructors)
            {
                yield return (e) =>
                {
                    string component = componentType.ToString();
                    object service = e.Context.Resolve(serviceType, new TypedParameter(typeof(Type), componentType));
                    var parameter = new TypedParameter(serviceType, service);
                    e.Parameters = e.Parameters.Concat(new[] { parameter });
                };
            }
        }

        private IEnumerable<Action<IComponentContext, object>> BuildPropertyServiceInjectors(Type componentType, Type serviceType)
        {
            var properties = componentType.GetPropertiesWithDependency(serviceType);

            foreach (var propertyInfo in properties)
            {
                yield return (ctx, instance) =>
                {
                    string component = componentType.ToString();
                    object service = ctx.Resolve(serviceType, new TypedParameter(typeof(Type), componentType));
                    propertyInfo.SetValue(instance, service, null);
                };
            }
        }
    }
}
