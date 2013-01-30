using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Builder;

namespace DAF.Core
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerWorkUnit<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> register)
        {
            return register.InstancePerMatchingLifetimeScope("workunit");
        }

        public static ILifetimeScope BeginWorkUnitScope(this IContainer container)
        {
            return container.BeginLifetimeScope("workunit");
        }
    }
}
