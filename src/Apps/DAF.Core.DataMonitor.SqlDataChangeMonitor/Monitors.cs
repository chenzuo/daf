using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            IOC.Current.Stop();
        }

        public static IEnumerable<IDataMonitor> DataMonitors
        {
            get { return IOC.Current.GetService<IEnumerable<IDataMonitor>>(); }
        }
    }
}
