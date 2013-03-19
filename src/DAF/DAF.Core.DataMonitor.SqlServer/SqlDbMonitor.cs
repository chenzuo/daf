using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DAF.Core.Data;
using DAF.Core.DataMonitor;

namespace DAF.Core.DataMonitor.SqlServer
{
    public class SqlDbMonitor : IDataMonitor
    {
        protected string connectionString;

        public SqlDbMonitor(string connStringName, string tableName, string columnNames, IChangeVersionManager versionManager, IEnumerable<IDataChangeHandler> handlers)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            VersionManager = versionManager;
            Handlers = handlers == null ? Enumerable.Empty<IDataChangeHandler>() : handlers.Where(o => o.ConnectionStringName == connStringName && o.TableName == tableName);
            ConnectionStringName = connStringName;
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }

        private DataOperation GetOperation(string op)
        {
            switch (op.ToLower())
            {
                case "i": return DataOperation.Insert;
                case "u": return DataOperation.Update;
                case "d": return DataOperation.Delete;
                default: return DataOperation.None;
            }
        }

        public void Start()
        {
            SqlDependency.Start(connectionString);
            VersionInfo lastUpdateVersion = VersionManager.GetLastVersion(TableName);
            if (lastUpdateVersion == null)
            {
                lastUpdateVersion = new VersionInfo() { TableName = TableName, LastUpdateVersion = 0, LastUpdateTime = DateTime.Now };
            }
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("SELECT {1} FROM dbo.[{0}]", TableName, ColumnNames); 
                        cmd.CommandType = System.Data.CommandType.Text;
                        conn.Open();
                        SqlDependency sd = new SqlDependency(cmd);
                        sd.OnChange += new OnChangeEventHandler((s, e) =>
                        {
                            if (e.Type == SqlNotificationType.Change)
                            {
                                using (var cn = new SqlConnection(connectionString))
                                {
                                    cn.Open();
                                    string cmdText = string.Format("SELECT * FROM CHANGETABLE(CHANGES {0}, {1}) CT", TableName, lastUpdateVersion.LastUpdateVersion);
                                    using (var cm = new SqlCommand(cmdText, cn))
                                    {
                                        using (var r = cm.ExecuteReader(CommandBehavior.CloseConnection))
                                        {
                                            List<ChangeInfo> citems = new List<ChangeInfo>();
                                            int fcount = r.FieldCount;
                                            int lastVersion = lastUpdateVersion.LastUpdateVersion;
                                            while (r.Read())
                                            {
                                                ChangeInfo item = new ChangeInfo();
                                                item.TableName = TableName;
                                                item.ChangeVersion = r["SYS_CHANGE_VERSION"].ConvertTo<int>(0);
                                                item.CreationVersion = r["SYS_CHANGE_CREATION_VERSION"].ConvertTo<int?>(null);
                                                item.Operation = GetOperation(r["SYS_CHANGE_OPERATION"].ToString());
                                                item.Key = r[fcount - 1].ToString();

                                                citems.Add(item);

                                                if (lastVersion < item.ChangeVersion)
                                                    lastVersion = item.ChangeVersion;
                                            }

                                            r.Close();
                                            if (Handlers != null)
                                            {
                                                Handlers.ForEach(h => h.HandleChange(citems));

                                                lastUpdateVersion.LastUpdateVersion = lastVersion;
                                                lastUpdateVersion.LastUpdateTime = DateTime.Now;
                                                VersionManager.SaveLastVersion(TableName, lastUpdateVersion);
                                            }
                                        }
                                    }
                                }
                            }
                        });

                        using (var r = cmd.ExecuteReader())
                        {
                            r.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Stop()
        {
            SqlDependency.Stop(connectionString);
        }

        public string ConnectionStringName { get; set; }
        public string TableName { get; set; }
        public string ColumnNames { get; set; }
        public IChangeVersionManager VersionManager { get; set; }
        public IEnumerable<IDataChangeHandler> Handlers { get; set; }
    }
}
