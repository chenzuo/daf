using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Autofac;
using DAF.Core;
using DAF.Core.Caching;

namespace DAF.Core.Data.EF
{
    public class DbContextFactory : IServiceFactory<DbContext>
    {
        public DbContextFactory()
        {
        }

        public DbContext Create(Type type, IComponentContext container)
        {
            Type entityType = type;
            if (type.IsGenericType)
                entityType = type.GetGenericArguments()[0];

            if (!DbContextConfig.EentityTypeToDbContextType.ContainsKey(entityType))
                throw new Exception(string.Format("type of {0} has not yet configered.", entityType.FullName));

            var dbType = DbContextConfig.EentityTypeToDbContextType[entityType];

            var db = Activator.CreateInstance(dbType) as DbContext;
            return db;
        }
    }
}
