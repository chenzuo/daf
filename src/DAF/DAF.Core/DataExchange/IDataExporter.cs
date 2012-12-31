using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public interface IDataExporter
    {
        string ConnectionString { get; set; }
        Type DataType { get; }
    }
}
