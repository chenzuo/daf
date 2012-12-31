using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public class ExportConfig
    {
        public string ExportId { get; set; }
        public string ExportName { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }

        public string Parameters { get; set; }

        public string DataTypeName { get; set; }
        public string ExporterTypeName { get; set; }
        public string SynchronizerTypeName { get; set; }
        public string PackagerTypeName { get; set; }
    }
}
