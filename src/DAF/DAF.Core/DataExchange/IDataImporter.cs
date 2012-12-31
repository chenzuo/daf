using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public interface IDataImporter
    {
        string ConnectionString { get; set; }
    }
}
