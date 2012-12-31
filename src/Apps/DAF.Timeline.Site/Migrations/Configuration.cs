namespace DAF.SSO.Server.Site.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.Timeline.Site.DB.EF.TimelineDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.Timeline.Site.DB.EF.TimelineDB context)
        {
        }
    }
}
