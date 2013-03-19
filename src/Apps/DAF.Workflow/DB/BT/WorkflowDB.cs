using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Data.BLToolkit;
using DAF.Workflow.Models;

namespace DAF.Workflow.DB.BT
{
    public class WorkflowDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "WorkflowDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(BizFlow),
                    typeof(NextBizFlow),
                    typeof(FlowState),
                    typeof(FlowOperation),
                    typeof(FlowIncome),
                    typeof(FlowOutcome),
                    typeof(FlowStateOperation),
                    typeof(FlowStateIncome),
                    typeof(FlowStateOutcome),
                    typeof(TargetFlow),
                    typeof(TargetState),
                    typeof(TargetIncome),
                    typeof(TargetOutcome),
                    typeof(NextTargetState)
                };
            }
        }
    }
}