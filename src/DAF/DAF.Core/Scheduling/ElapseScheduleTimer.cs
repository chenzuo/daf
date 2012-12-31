using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DAF.Core.Scheduling
{
    public class ElapseScheduleTimer : IScheduleTimer
    {
        private Timer timer;

        public ElapseScheduleTimer()
        {
        }

        public event ElapsedEventHandler Elapsed;

        public void Run(int interval)
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = TimeSpan.FromMinutes(interval).TotalMilliseconds;
            lock (timer)
            {
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!System.Threading.Monitor.TryEnter(timer))
                return;

            try
            {
                if (timer.Enabled)
                {
                    if (Elapsed != null)
                        Elapsed(sender, e);
                }
            }
            catch
            {
            }
            finally
            {
                System.Threading.Monitor.Exit(timer);
            }
        }

        public void Terminate()
        {
            lock (timer)
            {
                timer.Stop();
            }
            timer.Dispose();
        }
    }
}
