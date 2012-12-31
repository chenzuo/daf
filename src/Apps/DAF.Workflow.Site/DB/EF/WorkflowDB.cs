using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAF.Workflow.Models;

namespace DAF.App.Workflow.DB.EF
{
    public class WorkflowDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<BizFlow> BizFlow { get; set; }
        public DbSet<NextBizFlow> NextBizFlow { get; set; }
        public DbSet<FlowState> FlowState { get; set; }
        public DbSet<FlowOperation> FlowOperation { get; set; }
        public DbSet<FlowIncome> FlowIncome { get; set; }
        public DbSet<FlowOutcome> FlowOutcome { get; set; }
        public DbSet<FlowStateOperation> FlowStateOperation { get; set; }
        public DbSet<FlowStateIncome> FlowStateIncome { get; set; }
        public DbSet<FlowStateOutcome> FlowStateOutcome { get; set; }
        public DbSet<TargetFlow> TargetFlow { get; set; }
        public DbSet<TargetState> TargetState { get; set; }
        public DbSet<TargetIncome> TargetIncome { get; set; }
        public DbSet<TargetOutcome> TargetOutcome { get; set; }

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