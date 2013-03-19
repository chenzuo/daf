using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAF.Core.Search.Lucene
{
    public interface ILuceneLoader
    {
        string ConnectionStringName { get; set; }
        string ObjectTypeName { get; set; }
        string QuerySql { get; set; }
    }
}
