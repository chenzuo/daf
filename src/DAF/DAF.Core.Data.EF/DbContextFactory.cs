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
        protected ICacheProvider cache;

        public DbContextFactory(ICacheManager cacheManager)
        {
            this.cache = cacheManager.CreateCacheProvider(CacheScope.WorkUnit);
        }

        public DbContext Create(Type type, IComponentContext container)
        {
            Type entityType = type;
            if (type.IsGenericType)
                entityType = type.GetGenericArguments()[0];

            if (!DbContextConfig.EentityTypeToDbContextType.ContainsKey(entityType))
                throw new Exception(string.Format("type of {0} has not yet configered.", entityType.FullName));

            var dbType = DbContextConfig.EentityTypeToDbContextType[entityType];

            string key = dbType.FullName;
            if (cache.Contains(key))
                return cache.GetData(key) as DbContext;

            var db = Activator.CreateInstance(dbType) as DbContext;
            cache.Add(key, db);
            return db;
        }
    }
}
