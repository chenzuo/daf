using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataMonitor
{
    public interface IDataChangeHandler
    {
        string ConnectionStringName { get; }
        string TableName { get; }
        void HandleChange(IEnumerable<ChangeInfo> changedInfo);
    }
}
