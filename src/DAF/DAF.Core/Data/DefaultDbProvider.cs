using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace DAF.Core.Data
{
    public class DefaultDbProvider : IDbProvider
    {
        private DbProviderFactory dbFactory;
        private string connString;

        public void Initialize(string providerName, string connString)
        {
            dbFactory = DbProviderFactories.GetFactory(providerName);
            this.connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var conn = dbFactory.CreateConnection();
            conn.ConnectionString = connString;
            return conn;
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return dbFactory.CreateDataAdapter();
        }

        public IDbCommand CreateCommand(IDbConnection conn)
        {
            return conn.CreateCommand();
        }

        public IDbDataParameter CreateParameter(IDbCommand cmd)
        {
            return cmd.CreateParameter();
        }

        public void AttachParameter(IDbCommand cmd, IDbDataParameter para)
        {
            cmd.Parameters.Add(para);
        }

        public void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
        {
        }
    }
}
