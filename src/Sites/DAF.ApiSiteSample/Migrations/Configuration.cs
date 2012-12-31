namespace DAF.SSO.Server.Site.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.ApiSiteSample.DB.EF.SiteDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.ApiSiteSample.DB.EF.SiteDB context)
        {
        }
    }
}
