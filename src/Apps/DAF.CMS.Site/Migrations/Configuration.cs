namespace DAF.CMS.Site.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.CMS.Site.DB.EF.CMSDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.CMS.Site.DB.EF.CMSDB context)
        {
        }
    }
}
