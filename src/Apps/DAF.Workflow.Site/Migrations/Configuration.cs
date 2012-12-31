namespace DAF.App.Workflow.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.App.Workflow.DB.EF.WorkflowDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.App.Workflow.DB.EF.WorkflowDB context)
        {
        }
    }
}