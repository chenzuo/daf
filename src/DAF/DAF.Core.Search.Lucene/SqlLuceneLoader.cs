using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAF.Core.Search.Lucene
{
    public class SqlLuceneLoader : ILuceneLoader
    {
        public SqlLuceneLoader(string connStringName, string objTypeName, string querySql)
        {
            ConnectionStringName = connStringName;
            ObjectTypeName = objTypeName;
            QuerySql = querySql;
        }

        public string ConnectionStringName { get; set; }
        public string ObjectTypeName { get; set; }
        public string QuerySql { get; set; }
    }
}
