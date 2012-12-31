namespace DAF.MvcSiteSample.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.MvcSiteSample.DB.EF.SiteDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.MvcSiteSample.DB.EF.SiteDB context)
        {
        }
    }
}
