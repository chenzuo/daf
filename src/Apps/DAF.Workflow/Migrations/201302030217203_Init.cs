namespace DAF.Workflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wf_BizFlow",
                c => new
                    {
                        FlowId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        TargetType = c.String(maxLength: 50),
                        ClientId = c.String(maxLength: 50),
                        BizGroup = c.String(maxLength: 50),
                        Owner = c.String(maxLength: 50),
                        StartUrl = c.String(maxLength: 200),
                        DetailUrl = c.String(maxLength: 200),
                        Guide = c.String(maxLength: 500),
                        StopWhenIncomeRequired = c.Boolean(nullable: false),
                        StopWhenOutcomeRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FlowId);
            
            CreateTable(
                "dbo.wf_FlowState",
                c => new
                    {
                        StateId = c.String(nullable: false, maxLength: 50),
                        FlowId = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Guide = c.String(maxLength: 200),
                        IntervalType = c.Int(),
                        ResponseIntervalValue = c.Int(),
                        TreatIntervalValue = c.Int(),
                        StateType = c.Int(nullable: false),
                        Result = c.Int(),
                        AllParallelStateShouldBeEnd = c.Boolean(),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId)
                .Index(t => t.FlowId);
            
            CreateTable(
                "dbo.wf_FlowStateOperation",
                c => new
                    {
                        StateId = c.String(nullable: false, maxLength: 50),
                        OperationId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.StateId, t.OperationId })
                .ForeignKey("dbo.wf_FlowState", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.wf_FlowOperation", t => t.OperationId, cascadeDelete: true)
                .Index(t => t.StateId)
                .Index(t => t.OperationId);
            
            CreateTable(
                "dbo.wf_FlowOperation",
                c => new
                    {
                        OperationId = c.String(nullable: false, maxLength: 50),
                        FlowId = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Guide = c.String(maxLength: 200),
                        OperationUrl = c.String(maxLength: 200),
                        OperationArgs = c.String(maxLength: 200),
                        PermissionUri = c.String(maxLength: 200),
                        DefaultNextStateId = c.String(maxLength: 50),
                        CanPlanned = c.Boolean(nullable: false),
                        CanCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OperationId)
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId)
                .Index(t => t.FlowId);
            
            CreateTable(
                "dbo.wf_FlowStateIncome",
                c => new
                    {
                        StateId = c.String(nullable: false, maxLength: 50),
                        IncomeId = c.String(nullable: false, maxLength: 50),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StateId, t.IncomeId })
                .ForeignKey("dbo.wf_FlowState", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.wf_FlowIncome", t => t.IncomeId, cascadeDelete: true)
                .Index(t => t.StateId)
                .Index(t => t.IncomeId);
            
            CreateTable(
                "dbo.wf_FlowIncome",
                c => new
                    {
                        IncomeId = c.String(nullable: false, maxLength: 50),
                        FlowId = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        FileType = c.String(maxLength: 50),
                        SampleFileUrl = c.String(maxLength: 200),
                        UploadUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IncomeId)
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId)
                .Index(t => t.FlowId);
            
            CreateTable(
                "dbo.wf_FlowStateOutcome",
                c => new
                    {
                        StateId = c.String(nullable: false, maxLength: 50),
                        OutcomeId = c.String(nullable: false, maxLength: 50),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StateId, t.OutcomeId })
                .ForeignKey("dbo.wf_FlowState", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.wf_FlowOutcome", t => t.OutcomeId, cascadeDelete: true)
                .Index(t => t.StateId)
                .Index(t => t.OutcomeId);
            
            CreateTable(
                "dbo.wf_FlowOutcome",
                c => new
                    {
                        OutcomeId = c.String(nullable: false, maxLength: 50),
                        FlowId = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        FileType = c.String(maxLength: 50),
                        SampleFileUrl = c.String(maxLength: 200),
                        UploadUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.OutcomeId)
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId)
                .Index(t => t.FlowId);
            
            CreateTable(
                "dbo.wf_NextBizFlow",
                c => new
                    {
                        FlowId = c.String(nullable: false, maxLength: 50),
                        NextFlowId = c.String(nullable: false, maxLength: 50)
                    })
                .PrimaryKey(t => new { t.FlowId, t.NextFlowId })
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId, cascadeDelete: false)
                .ForeignKey("dbo.wf_BizFlow", t => t.NextFlowId, cascadeDelete: false)
                .Index(t => t.FlowId)
                .Index(t => t.NextFlowId);
            
            CreateTable(
                "dbo.wf_TargetFlow",
                c => new
                    {
                        TargetFlowId = c.String(nullable: false, maxLength: 50),
                        FlowId = c.String(nullable: false, maxLength: 50),
                        TargetId = c.String(maxLength: 50),
                        FlowCode = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Message = c.String(maxLength: 50),
                        HasStarted = c.Boolean(nullable: false),
                        HasCompleted = c.Boolean(nullable: false),
                        LastTargetFlowId = c.String(maxLength: 50),
                        CreatorId = c.String(maxLength: 50),
                        CreatorName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        Result = c.Int(),
                    })
                .PrimaryKey(t => t.TargetFlowId)
                .ForeignKey("dbo.wf_BizFlow", t => t.FlowId, cascadeDelete: true)
                .ForeignKey("dbo.wf_TargetFlow", t => t.LastTargetFlowId)
                .Index(t => t.FlowId)
                .Index(t => t.LastTargetFlowId);
            
            CreateTable(
                "dbo.wf_TargetState",
                c => new
                    {
                        TargetStateId = c.String(nullable: false, maxLength: 50),
                        TargetFlowId = c.String(maxLength: 50),
                        StateId = c.String(maxLength: 50),
                        OperationId = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Message = c.String(maxLength: 50),
                        ResponseExpiryTime = c.DateTime(),
                        TreatExpiryTime = c.DateTime(),
                        ResponsorId = c.String(maxLength: 50),
                        ResponsorName = c.String(maxLength: 50),
                        ResponseTime = c.DateTime(),
                        PlannerId = c.String(maxLength: 50),
                        PlannerName = c.String(maxLength: 50),
                        PlanTreatTime = c.DateTime(),
                        TreaterId = c.String(maxLength: 50),
                        TreaterName = c.String(maxLength: 50),
                        TreatTime = c.DateTime(),
                        OperatorId = c.String(maxLength: 50),
                        OperatorName = c.String(maxLength: 50),
                        OperateTime = c.DateTime(),
                        StateStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TargetStateId)
                .ForeignKey("dbo.wf_TargetFlow", t => t.TargetFlowId)
                .ForeignKey("dbo.wf_FlowOperation", t => t.OperationId)
                .ForeignKey("dbo.wf_FlowState", t => t.StateId)
                .Index(t => t.TargetFlowId)
                .Index(t => t.OperationId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.wf_TargetIncome",
                c => new
                    {
                        TargetIncomeId = c.String(nullable: false, maxLength: 50),
                        TargetStateId = c.String(maxLength: 50),
                        IncomeId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        UploaderId = c.String(maxLength: 50),
                        UploaderName = c.String(maxLength: 50),
                        UploadTime = c.DateTime(),
                        Verified = c.Boolean(),
                        VerifierId = c.String(maxLength: 50),
                        VerifierName = c.String(maxLength: 50),
                        VerifierTime = c.DateTime(),
                        FileType = c.String(maxLength: 50),
                        FileUrl = c.String(maxLength: 200),
                        FileStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TargetIncomeId)
                .ForeignKey("dbo.wf_TargetState", t => t.TargetStateId)
                .ForeignKey("dbo.wf_FlowIncome", t => t.IncomeId)
                .Index(t => t.TargetStateId)
                .Index(t => t.IncomeId);
            
            CreateTable(
                "dbo.wf_TargetOutcome",
                c => new
                    {
                        TargetOutcomeId = c.String(nullable: false, maxLength: 50),
                        TargetStateId = c.String(maxLength: 50),
                        OutcomeId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        UploaderId = c.String(maxLength: 50),
                        UploaderName = c.String(maxLength: 50),
                        UploadTime = c.DateTime(),
                        Verified = c.Boolean(),
                        VerifierId = c.String(maxLength: 50),
                        VerifierName = c.String(maxLength: 50),
                        VerifierTime = c.DateTime(),
                        FileType = c.String(maxLength: 50),
                        FileUrl = c.String(maxLength: 200),
                        FileStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TargetOutcomeId)
                .ForeignKey("dbo.wf_TargetState", t => t.TargetStateId)
                .ForeignKey("dbo.wf_FlowOutcome", t => t.OutcomeId)
                .Index(t => t.TargetStateId)
                .Index(t => t.OutcomeId);
            
            CreateTable(
                "dbo.wf_NextTargetState",
                c => new
                    {
                        TargetStateId = c.String(nullable: false, maxLength: 50),
                        NextTargetStateId = c.String(nullable: false, maxLength: 50),
                        ParallelTargetStateId = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => new { t.TargetStateId, t.NextTargetStateId })
                .ForeignKey("dbo.wf_TargetState", t => t.TargetStateId, cascadeDelete: false)
                .ForeignKey("dbo.wf_TargetState", t => t.NextTargetStateId, cascadeDelete: false)
                .ForeignKey("dbo.wf_TargetState", t => t.ParallelTargetStateId)
                .Index(t => t.TargetStateId)
                .Index(t => t.NextTargetStateId);
        }
        
        public override void Down()
        {
            DropIndex("dbo.wf_NextTargetState", new[] { "TargetState_TargetStateId1" });
            DropIndex("dbo.wf_NextTargetState", new[] { "TargetState_TargetStateId" });
            DropIndex("dbo.wf_NextTargetState", new[] { "ParallelTargetStateId" });
            DropIndex("dbo.wf_NextTargetState", new[] { "NextTargetStateId" });
            DropIndex("dbo.wf_NextTargetState", new[] { "TargetStateId" });
            DropIndex("dbo.wf_TargetOutcome", new[] { "OutcomeId" });
            DropIndex("dbo.wf_TargetOutcome", new[] { "TargetStateId" });
            DropIndex("dbo.wf_TargetIncome", new[] { "IncomeId" });
            DropIndex("dbo.wf_TargetIncome", new[] { "TargetStateId" });
            DropIndex("dbo.wf_TargetState", new[] { "StateId" });
            DropIndex("dbo.wf_TargetState", new[] { "OperationId" });
            DropIndex("dbo.wf_TargetState", new[] { "TargetFlowId" });
            DropIndex("dbo.wf_TargetFlow", new[] { "LastTargetFlowId" });
            DropIndex("dbo.wf_TargetFlow", new[] { "FlowId" });
            DropIndex("dbo.wf_NextBizFlow", new[] { "BizFlow_FlowId" });
            DropIndex("dbo.wf_NextBizFlow", new[] { "NextFlowId" });
            DropIndex("dbo.wf_NextBizFlow", new[] { "FlowId" });
            DropIndex("dbo.wf_FlowOutcome", new[] { "FlowId" });
            DropIndex("dbo.wf_FlowStateOutcome", new[] { "OutcomeId" });
            DropIndex("dbo.wf_FlowStateOutcome", new[] { "StateId" });
            DropIndex("dbo.wf_FlowIncome", new[] { "FlowId" });
            DropIndex("dbo.wf_FlowStateIncome", new[] { "IncomeId" });
            DropIndex("dbo.wf_FlowStateIncome", new[] { "StateId" });
            DropIndex("dbo.wf_FlowOperation", new[] { "FlowId" });
            DropIndex("dbo.wf_FlowStateOperation", new[] { "OperationId" });
            DropIndex("dbo.wf_FlowStateOperation", new[] { "StateId" });
            DropIndex("dbo.wf_FlowState", new[] { "FlowId" });
            DropForeignKey("dbo.wf_NextTargetState", "TargetState_TargetStateId1", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_NextTargetState", "TargetState_TargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_NextTargetState", "ParallelTargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_NextTargetState", "NextTargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_NextTargetState", "TargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_TargetOutcome", "OutcomeId", "dbo.wf_FlowOutcome");
            DropForeignKey("dbo.wf_TargetOutcome", "TargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_TargetIncome", "IncomeId", "dbo.wf_FlowIncome");
            DropForeignKey("dbo.wf_TargetIncome", "TargetStateId", "dbo.wf_TargetState");
            DropForeignKey("dbo.wf_TargetState", "StateId", "dbo.wf_FlowState");
            DropForeignKey("dbo.wf_TargetState", "OperationId", "dbo.wf_FlowOperation");
            DropForeignKey("dbo.wf_TargetState", "TargetFlowId", "dbo.wf_TargetFlow");
            DropForeignKey("dbo.wf_TargetFlow", "LastTargetFlowId", "dbo.wf_TargetFlow");
            DropForeignKey("dbo.wf_TargetFlow", "FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_NextBizFlow", "BizFlow_FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_NextBizFlow", "NextFlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_NextBizFlow", "FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_FlowOutcome", "FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_FlowStateOutcome", "OutcomeId", "dbo.wf_FlowOutcome");
            DropForeignKey("dbo.wf_FlowStateOutcome", "StateId", "dbo.wf_FlowState");
            DropForeignKey("dbo.wf_FlowIncome", "FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_FlowStateIncome", "IncomeId", "dbo.wf_FlowIncome");
            DropForeignKey("dbo.wf_FlowStateIncome", "StateId", "dbo.wf_FlowState");
            DropForeignKey("dbo.wf_FlowOperation", "FlowId", "dbo.wf_BizFlow");
            DropForeignKey("dbo.wf_FlowStateOperation", "OperationId", "dbo.wf_FlowOperation");
            DropForeignKey("dbo.wf_FlowStateOperation", "StateId", "dbo.wf_FlowState");
            DropForeignKey("dbo.wf_FlowState", "FlowId", "dbo.wf_BizFlow");
            DropTable("dbo.wf_NextTargetState");
            DropTable("dbo.wf_TargetOutcome");
            DropTable("dbo.wf_TargetIncome");
            DropTable("dbo.wf_TargetState");
            DropTable("dbo.wf_TargetFlow");
            DropTable("dbo.wf_NextBizFlow");
            DropTable("dbo.wf_FlowOutcome");
            DropTable("dbo.wf_FlowStateOutcome");
            DropTable("dbo.wf_FlowIncome");
            DropTable("dbo.wf_FlowStateIncome");
            DropTable("dbo.wf_FlowOperation");
            DropTable("dbo.wf_FlowStateOperation");
            DropTable("dbo.wf_FlowState");
            DropTable("dbo.wf_BizFlow");
        }
    }
}
