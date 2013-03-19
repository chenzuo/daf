using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataMonitor
{
    public class VersionInfo
    {
        public string TableName { get; set; }
        public int LastUpdateVersion { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
