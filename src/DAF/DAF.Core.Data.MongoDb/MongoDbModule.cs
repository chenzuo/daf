using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DAF.Core;

namespace DAF.Core.Data.MongoDb
{
    public class MongoDbModule : TypeBasedModule<MongoDatabase>
    {
        public MongoDbModule()
            : base(BuildContainerRegisterations)
        {
        }

        private static void BuildContainerRegisterations(ContainerBuilder builder)
        {
            builder.RegisterType<SqlFileExecutor>().As<ISqlFileExecutor>();
            builder.RegisterType<DatabaseFactory>().As<IServiceFactory<MongoDatabase>>();
            builder.RegisterGeneric(typeof(CollectionRepository<>)).As(typeof(IRepository<>));
        }

        protected override bool CacheService
        {
            get
            {
                return false;
            }
        }
    }
}
