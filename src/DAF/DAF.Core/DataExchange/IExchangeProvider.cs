using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.DataExchange
{
    public interface IExchangeProvider
    {
        void Initialize();
        void InitEnv(ExportConfig config);
        void InitEnv(ImportConfig config);

        ExchangePackage Export(ExportConfig config);
        bool Import(ExchangePackage package, ImportConfig config);
    }
}
