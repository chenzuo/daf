using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace DAF.Core.Data
{
    public interface IDbProvider
    {
        void Initialize(string providerName, string connString);
        IDbConnection CreateConnection();
        IDbDataAdapter CreateDataAdapter();
        IDbCommand CreateCommand(IDbConnection conn);
        IDbDataParameter CreateParameter(IDbCommand cmd);
        void AttachParameter(IDbCommand cmd, IDbDataParameter para);
        void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters);
    }

    public static class IDbProviderExtensions
    {
        public static void Initialize(this IDbProvider dbProvider, string connSettingsName)
        {
            var connSettings = ConfigurationManager.ConnectionStrings[connSettingsName];
            dbProvider.Initialize(connSettings.ProviderName, connSettings.ConnectionString);
        }

        public static IDbCommand CreateCommand(this IDbProvider dbProvider, string sql, CommandType cmdType = CommandType.Text, Func<IDbCommand, IDbDataParameter[]> getParas = null)
        {
            var conn = dbProvider.CreateConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trans = conn.BeginTransaction();
            var cmd = dbProvider.CreateCommand(conn);

            IDbDataParameter[] paras = null;
            if (getParas != null)
                paras = getParas(cmd);

            dbProvider.PrepareCommand(ref cmdType, ref sql, ref paras);
            cmd.CommandType = cmdType;
            cmd.CommandText = sql;

            if (paras != null && paras.Length > 0)
            {
                foreach (var p in paras)
                {
                    dbProvider.AttachParameter(cmd, p);
                }
            }
            cmd.Transaction = trans;
            return cmd;
        }
    }
}
