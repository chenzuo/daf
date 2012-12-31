using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.DataExchange
{
    public class ExchangePackage
    {
        public string PackageId { get; set; }
        public string DataTypeName { get; set; }
        public string ZipFileName { get; set; }

        public string ExportAppName { get; set; }
        public string ExportUserName { get; set; }
        public DateTime? ExportTime { get; set; }

        public string ImportAppName { get; set; }
        public string ImportUserName { get; set; }
        public DateTime? ImportTime { get; set; }
    }
}
