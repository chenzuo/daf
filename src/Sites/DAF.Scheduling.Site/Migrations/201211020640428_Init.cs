namespace DAF.Scheduling.Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.schedule_Task",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 200),
                        OperationName = c.String(maxLength: 50),
                        Enabled = c.Boolean(nullable: false),
                        RetryTimes = c.Int(nullable: false),
                        StartParameters = c.String(maxLength: 200),
                        ActiveParameters = c.String(maxLength: 200),
                        StopParameters = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.schedule_TaskTrigger",
                c => new
                    {
                        TaskTriggerId = c.String(nullable: false, maxLength: 50),
                        TaskName = c.String(nullable: false, maxLength: 50),
                        TriggerName = c.String(nullable: false, maxLength: 50),
                        Enabled = c.Boolean(nullable: false),
                        BeginTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        InitTriggerValue = c.String(maxLength: 200),
                        LastTriggerValue = c.String(maxLength: 200),
                        TriggerParameters = c.String(maxLength: 200),
                        LastActiveTime = c.DateTime(),
                        RetryTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskTriggerId)
                .ForeignKey("dbo.schedule_Task", t => t.TaskName, cascadeDelete: true)
                .Index(t => t.TaskName);
            
            CreateTable(
                "dbo.schedule_TaskExecuteLog",
                c => new
                    {
                        LogId = c.String(nullable: false, maxLength: 50),
                        TaskName = c.String(nullable: false, maxLength: 50),
                        OperationName = c.String(nullable: false, maxLength: 50),
                        TriggerName = c.String(nullable: false, maxLength: 50),
                        ActiveRemark = c.String(),
                        ActiveParameters = c.String(maxLength: 200),
                        TriggerTime = c.DateTime(nullable: false),
                        ExecuteTime = c.DateTime(nullable: false),
                        RetryTimes = c.Int(nullable: false),
                        Exception = c.String(),
                        Success = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.schedule_TaskTrigger", new[] { "TaskName" });
            DropForeignKey("dbo.schedule_TaskTrigger", "TaskName", "dbo.schedule_Task");
            DropTable("dbo.schedule_TaskExecuteLog");
            DropTable("dbo.schedule_TaskTrigger");
            DropTable("dbo.schedule_Task");
        }
    }
}
