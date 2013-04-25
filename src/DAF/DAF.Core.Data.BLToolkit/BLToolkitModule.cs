using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.Reflection.MetadataProvider;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Core.Data.BLToolkit
{
    public class BLToolkitModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IAppEventHandler, BLToolkitDataEventHandler>(name: "BLToolkitDataEventHandler");

            builder.RegisterType<IDbProvider, DbProvider>();
            builder.RegisterType<ISqlFileExecutor, SqlFileExecutor>();
            builder.RegisterFactory<IDataContext, DataContextFactory>();
            builder.RegisterGeneric(typeof(IRepository<>), typeof(DataContextRepository<>));
        }
    }
}
