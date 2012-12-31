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

        public static T ExecuteCommand<T>(this IDbProvider dbProvider, IDbCommand cmd, Func<IDbCommand, T> execute)
        {
            var trans = cmd.Transaction;
            try
            {
                T result = execute(cmd);

                trans.Commit();
                return result;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }
        public static T ExecuteAdapter<T>(this IDbProvider dbProvider, IDbCommand cmd, Func<IDbDataAdapter, T> execute)
        {
            var trans = cmd.Transaction;
            try
            {
                var da = dbProvider.CreateDataAdapter();
                da.SelectCommand = cmd;
                T result = execute(da);

                trans.Commit();
                return result;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }
        public static int ExecuteNonQuery(this IDbProvider dbProvider, IDbCommand cmd)
        {
            return dbProvider.ExecuteCommand<int>(cmd, c =>
            {
                return c.ExecuteNonQuery();
            });
        }

        public static DataSet ExecuteDataSet(this IDbProvider dbProvider, IDbCommand cmd)
        {
            return dbProvider.ExecuteAdapter<DataSet>(cmd, c =>
            {
                DataSet ds = new DataSet();
                c.Fill(ds);
                return ds;
            });
        }

        public static DataTable ExecuteDataTable(this IDbProvider dbProvider, IDbCommand cmd)
        {
            return dbProvider.ExecuteAdapter<DataTable>(cmd, c =>
            {
                DataSet ds = new DataSet();
                c.Fill(ds);
                return ds.Tables[0];
            });
        }

        public static IDataReader ExecuteDataReader(this IDbProvider dbProvider, IDbCommand cmd)
        {
            return dbProvider.ExecuteCommand<IDataReader>(cmd, c =>
            {
                return c.ExecuteReader(CommandBehavior.CloseConnection);
            });
        }

        public static object ExecuteScalar(this IDbProvider dbProvider, IDbCommand cmd)
        {
            return dbProvider.ExecuteCommand<object>(cmd, c =>
            {
                return c.ExecuteScalar();
            });
        }
    }
}
