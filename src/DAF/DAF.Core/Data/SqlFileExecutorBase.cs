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
    public abstract class SqlFileExecutorBase : ISqlFileExecutor
    {
        protected string connString;

        protected IDbProvider dbProvider;

        protected abstract IDbProvider GetDbProvider(string providerName);

        public virtual void Initialize(string providerName, string connString)
        {
            this.connString = connString;
            this.dbProvider = GetDbProvider(providerName);
        }

        public virtual bool ExecuteSqlFile(string spliter, string sqlFile, bool continueOnError, out string messages)
        {
            messages = string.Empty;

            var conn = dbProvider.CreateConnection();
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                List<string> sqls = AnalysisSqlFile(spliter, sqlFile);
                if (sqls.Count > 0)
                {
                    foreach (var sql in sqls)
                    {
                        try
                        {
                            var cmd = dbProvider.CreateCommand(conn);
                            var cmdType = CommandType.Text;
                            IDbDataParameter[] dbParas = null;
                            var cmdText = sql;
                            dbProvider.PrepareCommand(ref cmdType, ref cmdText, ref dbParas);
                            cmd.CommandText = cmdText;
                            cmd.CommandType = cmdType;
                            cmd.Transaction = trans;
                            cmd.ExecuteNonQuery();
                            messages += string.Format("{0}Successfully executed:{0}{1}", Environment.NewLine, sql);
                        }
                        catch (Exception ex)
                        {
                            messages += string.Format("{0}Error:{0}{1}{0} throws on executing:{0}{2}", Environment.NewLine, ex.Message, sql);
                            if (continueOnError)
                                continue;
                            else
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                    }
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        protected virtual List<string> AnalysisSqlFile(string spliter, string sqlFile)
        {
            StreamReader sr = new StreamReader(sqlFile);
            List<string> sqls = new List<string>();
            string sql = string.Empty;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.EndsWith(spliter))
                {
                    if (line.Length > spliter.Length)
                    {
                        sql += Environment.NewLine + line.Substring(0, line.Length - spliter.Length);
                    }

                    sqls.Add(sql);
                    sql = string.Empty;
                }
                else
                {
                    sql += Environment.NewLine + line;
                }
            }

            sr.Close();
            sr.Dispose();
            sr = null;

            return sqls;
        }
    }
}
