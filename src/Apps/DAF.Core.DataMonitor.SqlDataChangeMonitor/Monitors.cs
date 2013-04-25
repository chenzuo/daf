using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAF.Core.IOC;

namespace DAF.Core.DataMonitor.SqlDataChangeMonitor
{
    public class Monitors
    {
        public static void Start()
        {
            if (DataMonitors != null)
            {
                foreach (var m in DataMonitors)
                {
                    m.Start();
                }
            }
        }

        public static void Stop()
        {
            if (DataMonitors != null)
            {
                foreach (var m in DataMonitors)
                {
                    m.Stop();
                }
            }

            IocInstance.Stop();
        }

        public static IEnumerable<IDataMonitor> DataMonitors
        {
            get { return IocInstance.Container.Resolve<IEnumerable<IDataMonitor>>(); }
        }
    }
}
