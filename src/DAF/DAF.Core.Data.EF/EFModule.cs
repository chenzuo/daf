using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DAF.Core.IOC;
using DAF.Core.Data;

namespace DAF.Core.Data.EF
{
    public class EFModule: IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IDbProvider, DefaultDbProvider>();
            builder.RegisterType<ISqlFileExecutor, DefaultSqlFileExecutor>();
            builder.RegisterFactory<DbContext, DbContextFactory>();
            builder.RegisterGeneric(typeof(IRepository<>), typeof(DbContextRepository<>));
        }
    }
}

