using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.DataExchange
{
    public class ImportConfig
    {
        public string ImportId { get; set; }
        public string ImportName { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }

        public bool InsertIfNotFound { get; set; }
        public bool DeleteIfNotMatched { get; set; }
        public bool ImportInSameTransaction { get; set; }

        public string Parameters { get; set; }

        public string DataTypeName { get; set; }
        public string ImporterTypeName { get; set; }
        public string SynchronizerTypeName { get; set; }
        public string PackagerTypeName { get; set; }
    }
}
