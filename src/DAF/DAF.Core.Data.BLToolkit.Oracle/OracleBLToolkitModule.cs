using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.Reflection.MetadataProvider;
using DAF.Core;

namespace DAF.Core.Data.BLToolkit.Oracle
{
    public class OracleBLToolkitModule : TypeBasedModule<IDataContext>
    {
        public OracleBLToolkitModule()
            : base(BuildContainerRegisterations)
        {
        }

        private static void BuildContainerRegisterations(ContainerBuilder builder)
        {
            builder.RegisterType<OracleBLToolkitDataEventHandler>()
                .As<IAppEventHandler>()
                .Named<IAppEventHandler>("BLToolkitDataEventHandler");

            builder.RegisterType<OracleDbProvider>().As<IDbProvider>();
            builder.RegisterType<SqlFileExecutor>().As<ISqlFileExecutor>();
            builder.RegisterType<DataContextFactory>().As<IServiceFactory<IDataContext>>();
            builder.RegisterGeneric(typeof(DataContextRepository<>)).As(typeof(IRepository<>));
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
