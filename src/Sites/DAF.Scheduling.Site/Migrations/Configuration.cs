namespace DAF.Scheduling.Site.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.Scheduling.Site.DB.EF.ScheduleDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.Scheduling.Site.DB.EF.ScheduleDB context)
        {
        }
    }
}
