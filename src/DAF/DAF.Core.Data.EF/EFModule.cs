using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Autofac;
using DAF.Core.Data;

namespace DAF.Core.Data.EF
{
    public class EFModule: TypeBasedModule<DbContext>
    {
        public EFModule()
            : base(BuildContainerRegisterations)
        {
        }

        private static void BuildContainerRegisterations(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultDbProvider>().As<IDbProvider>();
            builder.RegisterType<DefaultSqlFileExecutor>().As<ISqlFileExecutor>();
            builder.RegisterType<DbContextFactory>().As<IServiceFactory<DbContext>>();
            builder.RegisterGeneric(typeof(DbContextRepository<>)).As(typeof(IRepository<>));
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

