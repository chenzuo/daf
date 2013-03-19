using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Data;

namespace DAF.Core.DataMonitor
{
    public class ChangeInfo
    {
        public string TableName { get; set; }
        public DataOperation Operation { get; set; }
        public int ChangeVersion { get; set; }
        public int? CreationVersion { get; set; }
        public string Key { get; set; }
    }
}
