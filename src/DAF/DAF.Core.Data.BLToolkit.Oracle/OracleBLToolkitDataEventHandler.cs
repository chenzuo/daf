using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using DAF.Core.IOC;

namespace DAF.Core.Data.BLToolkit.Oracle
{
    public class OracleBLToolkitDataEventHandler : BLToolkitDataEventHandler
    {
        public override void OnApplicationStart(IIocContainer container, object context)
        {
            DbManager.AddDataProvider("Oracle", new OdpDataProvider());
            base.OnApplicationStart(container, context);
        }
    }
}
