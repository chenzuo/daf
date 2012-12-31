using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace DAF.Core.Data.BLToolkit
{
    public class DbProvider : IDbProvider
    {
        protected DataProviderBase dp;
        protected string connString;

        public virtual void Initialize(string providerName, string connString)
        {
            dp = DbManager.GetDataProvider(providerName);
            this.connString = connString;
        }

        public virtual IDbConnection CreateConnection()
        {
            var conn = dp.CreateConnectionObject();
            conn.ConnectionString = connString;
            return conn;
        }

        public virtual IDbDataAdapter CreateDataAdapter()
        {
            return dp.CreateDataAdapterObject();
        }

        public virtual IDbCommand CreateCommand(IDbConnection conn)
        {
            return dp.CreateCommandObject(conn);
        }

        public virtual IDbDataParameter CreateParameter(IDbCommand cmd)
        {
            var para = dp.CreateParameterObject(cmd);
            return para;
        }

        public virtual void AttachParameter(IDbCommand cmd, IDbDataParameter para)
        {
            dp.AttachParameter(cmd, para);
        }

        public virtual void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
        {
            dp.PrepareCommand(ref commandType, ref commandText, ref commandParameters);
        }
    }
}
