using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataMonitor
{
    public interface IChangeVersionManager
    {
        VersionInfo GetLastVersion(string tableName);
        void SaveLastVersion(string tableName, VersionInfo lastVersion);
    }
}
