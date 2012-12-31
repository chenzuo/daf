using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DAF.Core.Scheduling.Provider;

namespace DAF.Scheduling.Site.DB.EF
{
    public class ScheduleDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<ScheduleTask> Tasks { get; set; }
        public DbSet<ScheduleTaskTrigger> TaskTriggers { get; set; }
        public DbSet<SheduleTaskExecuteLog> TaskExecuteLogs { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer<ScheduleDB>(null);
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Later; }
        }
    }
}