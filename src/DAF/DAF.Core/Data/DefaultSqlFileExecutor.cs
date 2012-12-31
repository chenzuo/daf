using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Configuration;

namespace DAF.Core.Data
{
    public class DefaultSqlFileExecutor : SqlFileExecutorBase
    {
        protected override IDbProvider GetDbProvider(string providerName)
        {
            var dbProvider = new DefaultDbProvider();
            dbProvider.Initialize(providerName);
            return dbProvider;
        }
    }
}
