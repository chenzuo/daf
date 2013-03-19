using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DAF.Core.Data;

namespace DAF.Core.Search.Lucene
{
    public class LuceneBatchIndexWriter : IBatchIndexWriter
    {
        private IEnumerable<ILuceneLoader> loaders;
        private IDbProvider dbProvider;
        private ISearchProvider searchProvider;

        public LuceneBatchIndexWriter(IEnumerable<ILuceneLoader> loaders, IDbProvider dbProvider, ISearchProvider searchProvider)
        {
            this.loaders = loaders;
            this.dbProvider = dbProvider;
            this.searchProvider = searchProvider;
        }

        public void Run()
        {
            Parallel.ForEach(loaders, loader =>
                {
                    dbProvider.Initialize(loader.ConnectionStringName);
                    List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();
                    using (var cmd = dbProvider.CreateCommand(loader.QuerySql))
                    {
                        using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> dic = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var fn = reader.GetName(i);
                                    var val = reader[i];
                                    dic.Add(fn, val);
                                }
                                objs.Add(dic);
                            }
                            reader.Close();
                        }
                    }

                    var writer = searchProvider.CreateIndex(loader.ObjectTypeName);
                    foreach (var obj in objs)
                    {
                        writer.AddDocument(obj);
                    }
                    writer.Save();
                });
        }
    }
}
