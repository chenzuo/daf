using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLToolkit;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using DAF.Core.Data;

namespace DAF.Core.Data.BLToolkit
{
    public class SqlFileExecutor : SqlFileExecutorBase
    {
        protected override IDbProvider GetDbProvider(string providerName)
        {
            var dp = new DbProvider();
            dp.Initialize(providerName);
            return dp;
        }
    }
}
