using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Builder;
using DAF.Core;

namespace DAF.Core
{
    public class TypeBasedModule<IService> : Autofac.Module
    {
        protected readonly ConcurrentDictionary<string, IService> serviceCache;
        protected Action<ContainerBuilder> registration;

        public TypeBasedModule(Action<ContainerBuilder> registration)
        {
            serviceCache = new ConcurrentDictionary<string, IService>();
            this.registration = registration;
        }

        protected virtual Type DetermineContainingType(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericArguments()[0];
            return type;
        }

        protected virtual void LifeTimeScope<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            registration(builder);

            // call Create in response to the request for an IService implementation
            var rb = builder.Register((c, p) =>
            {
                Type containingType = typeof(object);
                if (p.Count() > 0)
                    containingType = p.TypedAs<Type>();

                containingType = DetermineContainingType(containingType);
                var factory = c.Resolve<IServiceFactory<IService>>();
                return factory.Create(containingType, c);
            }).As<IService>();

            LifeTimeScope(rb);
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var implementationType = registration.Activator.LimitType;

            var constructorInjectors = BuildConstructorServiceInjectors(implementationType);
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
            var injectors = BuildPropertyServiceInjectors(implementationType).ToArray();

            if (injectors.Any())
            {
                registration.Activated += (s, e) =>
                {
                    foreach (var injector in injectors)
                        injector(e.Context, e.Instance);
                };
            }
        }

        protected virtual IService GetService(IComponentContext context, Type componentType)
        {
            return context.Resolve<IService>(new TypedParameter(typeof(Type), componentType));
        }

        private IEnumerable<Action<PreparingEventArgs>> BuildConstructorServiceInjectors(Type componentType)
        {
            var constructors = componentType.GetConstructorsWithDependency(typeof(IService));
            foreach (var constructInfo in constructors)
            {
                yield return (e) =>
                {
                    string component = componentType.ToString();
                    IService service;
                    if (CacheService)
                        service = serviceCache.GetOrAdd(component, key => GetService(e.Context, componentType));
                    else
                        service = GetService(e.Context, componentType);
                    var parameter = new TypedParameter(typeof(IService), service);
                    e.Parameters = e.Parameters.Concat(new[] { parameter });
                };
            }
        }

        private IEnumerable<Action<IComponentContext, object>> BuildPropertyServiceInjectors(Type componentType)
        {
            // Look for settable properties of type "IService" 
            var properties = componentType.GetPropertiesWithDependency(typeof(IService));

            // Return an array of actions that resolve a Service and assign the property
            foreach (var propertyInfo in properties)
            {
                yield return (ctx, instance) =>
                {
                    string component = componentType.ToString();
                    IService service;
                    if (CacheService)
                        service = serviceCache.GetOrAdd(component, key => GetService(ctx, componentType));
                    else
                        service = GetService(ctx, componentType);
                    propertyInfo.SetValue(instance, service, null);
                };
            }
        }

        protected virtual bool CacheService
        {
            get { return true; }
        }
    }
}
