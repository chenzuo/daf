using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Core.Data.EF
{
    public class AutoMapTypeToDbContext : IAutoRegister
    {
        public void Register(IIocBuilder builder, Type type)
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
