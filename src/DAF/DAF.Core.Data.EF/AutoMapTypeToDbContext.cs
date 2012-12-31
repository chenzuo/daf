using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity;
using Autofac;
using DAF.Core;

namespace DAF.Core.Data.EF
{
    public class AutoMapTypeToDbContext : IAutoRegisterContainerWithType
    {
        public void Register(ContainerBuilder builder, Type type)
        {
            if (!type.IsAbstract && type.IsClass && typeof(DbContext).IsAssignableFrom(type))
            {
                var props = type.GetProperties().Where(o => o.PropertyType.IsGenericType && o.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>)));
                foreach (var prop in props)
                {
                    var entityType = prop.PropertyType.GetGenericArguments()[0];
                    DbContextConfig.EentityTypeToDbContextType.Add(entityType, type);
                }
            }
        }
    }
}
