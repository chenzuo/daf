using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;

namespace DAF.Core
{
    public class AutoWireModule<IService> : Autofac.Module
    {
        protected Action<ContainerBuilder> registration;

        public AutoWireModule(Action<ContainerBuilder> registration)
        {
            this.registration = registration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            registration(builder);
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var hasConstructorDependency = registration.Activator.LimitType.GetConstructorsWithDependency(typeof(IService)).Any();

            if (hasConstructorDependency)
            {
                registration.Preparing += (sender, e) =>
                {
                    var parameter = new TypedParameter(
                        typeof(IService),
                        e.Context.Resolve<IService>(new TypedParameter(typeof(Type), registration.Activator.LimitType)));
                    e.Parameters = e.Parameters.Concat(new[] { parameter });
                };
            }
            else
            {
                var props = registration.Activator.LimitType.GetPropertiesWithDependency(typeof(IService));
                if (props.Any())
                {
                    registration.Activated += (s, e) =>
                    {
                        foreach (var prop in props)
                        {
                            prop.SetValue(e.Instance,
                                e.Context.Resolve<IService>(), null);
                        }
                    };
                }
            }
        }
    }
}
