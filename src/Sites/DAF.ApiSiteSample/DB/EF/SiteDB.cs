using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;

namespace DAF.ApiSiteSample.DB.EF
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