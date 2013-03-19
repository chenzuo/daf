using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataMonitor.SqlDataChangeMonitor
{
    public partial class MonitorService : ServiceBase
    {
        public MonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Monitors.Start();
        }

        protected override void OnStop()
        {
            Monitors.Stop();
        }
    }
}
