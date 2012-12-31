using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace DAF.Core.Data.BLToolkit.Oracle
{
    public class OracleDbProvider : DbProvider
    {
        public override IDbDataParameter CreateParameter(IDbCommand cmd)
        {
            var para = dp.CreateParameterObject(cmd);
            if (para is OdpDataProvider.OracleParameterWrap)
            {
                return (para as OdpDataProvider.OracleParameterWrap).OracleParameter as IDbDataParameter;
            }
            return para;
        }
    }
}
