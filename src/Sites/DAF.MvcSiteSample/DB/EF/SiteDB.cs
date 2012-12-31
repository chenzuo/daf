using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DAF.MvcSiteSample.DB.EF
{
    public class SiteDB : DbContext, DAF.Core.IStartup
    {
        //public DbSet<Object> Objects { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer<SiteDB>(null);
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Later; }
        }
    }
}