using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Autofac;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Caching;

namespace DAF.Core.Data.MongoDb
{
    public class DatabaseFactory : IServiceFactory<MongoDatabase>
    {
        protected IEnumerable<IEntitySet> entitySets;

        public DatabaseFactory(IEnumerable<IEntitySet> entitySets)
        {
            this.entitySets = entitySets;
        }

        public MongoDatabase Create(Type type, IComponentContext container)
        {
            Type entityType = type;
            if (type.IsGenericType)
                entityType = type.GetGenericArguments()[0];

            string connString = null;
            string dbName = null;
            var entitySet = entitySets.FirstOrDefault(o => o.EntityTypes.Any(e => e.Equals(entityType)));
            if (entitySet != null)
            {
                connString = ConfigurationManager.ConnectionStrings[entitySet.ConnectionString].ConnectionString;
                dbName = entitySet.ConnectionString;
            }
            else
            {
                connString = ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString;
                dbName = type.AssemblyName().Replace(".", "_");
            }

            var server = MongoServer.Create(connString); 
            var db = server.GetDatabase(dbName);
            return db;
        }
    }
}
