using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAF.Core.Data
{
    public interface ISqlFileExecutor
    {
        void Initialize(string providerName, string connString);
        bool ExecuteSqlFile(string spliter, string sqlFile, bool continueOnError, out string messages);
    }
}
