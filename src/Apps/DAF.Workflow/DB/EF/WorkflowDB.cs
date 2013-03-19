using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAF.Workflow.Models;

namespace DAF.Workflow.DB.EF
{
    public class WorkflowDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<BizFlow> BizFlows { get; set; }
        public DbSet<NextBizFlow> NextBizFlows { get; set; }
        public DbSet<FlowState> FlowStates { get; set; }
        public DbSet<FlowOperation> FlowOperations { get; set; }
        public DbSet<FlowIncome> FlowIncomes { get; set; }
        public DbSet<FlowOutcome> FlowOutcomes { get; set; }
        public DbSet<FlowStateOperation> FlowStateOperations { get; set; }
        public DbSet<FlowStateIncome> FlowStateIncomes { get; set; }
        public DbSet<FlowStateOutcome> FlowStateOutcomes { get; set; }
        public DbSet<TargetFlow> TargetFlows { get; set; }
        public DbSet<TargetState> TargetStates { get; set; }
        public DbSet<TargetIncome> TargetIncomes { get; set; }
        public DbSet<TargetOutcome> TargetOutcomes { get; set; }
        public DbSet<NextTargetState> NextTargetStates { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WorkflowDB, Migrations.Configuration>());
        }

        public int ExecuteOrder
        {
            get { return DAF.Core.ExecuteOrder.Later; }
        }
    }
}