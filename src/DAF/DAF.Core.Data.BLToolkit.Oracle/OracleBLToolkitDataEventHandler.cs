using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace DAF.Core.Data.BLToolkit.Oracle
{
    public class OracleBLToolkitDataEventHandler : BLToolkitDataEventHandler
    {
        public override void OnApplicationStart(IContainer container, object context)
        {
            DbManager.AddDataProvider("Oracle", new OdpDataProvider());
            base.OnApplicationStart(container, context);
        }
    }
}
