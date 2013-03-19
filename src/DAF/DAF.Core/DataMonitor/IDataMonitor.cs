using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataMonitor
{
    public interface IDataMonitor
    {
        string ConnectionStringName { get; set; }
        string TableName { get; set; }
        string ColumnNames { get; set; }
        IChangeVersionManager VersionManager { get; set; }
        IEnumerable<IDataChangeHandler> Handlers { get;set;}

        void Start();
        void Stop();
    }
}
