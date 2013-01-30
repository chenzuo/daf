using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.Mapping;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Caching;

namespace DAF.Core.Data.BLToolkit
{
    public class DataContextFactory : IServiceFactory<IDataContext>
    {
        protected IEnumerable<IEntitySet> entitySets;

        public DataContextFactory(IEnumerable<IEntitySet> entitySets)
        {
            this.entitySets = entitySets;
        }

        public IDataContext Create(Type type, IComponentContext container)
        {
            Type entityType = type;
            if (type.IsGenericType)
                entityType = type.GetGenericArguments()[0];

            var entitySet = entitySets.FirstOrDefault(o => o.EntityTypes.Any(e => e.Equals(entityType)));
            if (entitySet == null)
                throw new Exception(string.Format("type of {0} has not yet configered.", entityType.FullName));

            var db = new DataContext(entitySet.ConnectionString);
            ((DataContext)db).MappingSchema = new DefaultMappingSchema();

            return db;
        }
    }
}
