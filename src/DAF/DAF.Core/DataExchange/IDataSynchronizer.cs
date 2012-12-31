using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public interface IDataSynchronizer
    {
        void InitExportMarker(string objectType);
        ExportMark GetLastExportSyncMark(string objectType);
        bool MarkPreExportSync(string packageId, string objectType);
        bool MarkPostExportSync(string packageId, string objectType);

        void InitImportMarker(string objectType);
        ImportMark GetLastImportSyncMark(string objectType);
        bool MarkPreImportSync(string packageId, string objectType);
        bool MarkPostImportSync(string packageId, string objectType);

        string ConnectionString { get; set; }
    }
}
