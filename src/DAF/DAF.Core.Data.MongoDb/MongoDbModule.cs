using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Core.Data.MongoDb
{
    public class MongoDbModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<ISqlFileExecutor, SqlFileExecutor>();
            builder.RegisterFactory<MongoDatabase, DatabaseFactory>();
            builder.RegisterGeneric(typeof(IRepository<>), typeof(CollectionRepository<>));
        }
    }
}
