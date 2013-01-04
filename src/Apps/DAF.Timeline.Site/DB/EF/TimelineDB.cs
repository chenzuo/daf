using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;
using DAF.Timeline.Models;

namespace DAF.Timeline.Site.DB.EF
{
    public class TimelineDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<TimelineItem> TimelineItems { get; set; }
        public DbSet<TimelineItemHistory> TimelineItemHistories { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer<TimelineDB>(null);
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Later; }
        }
    }
}