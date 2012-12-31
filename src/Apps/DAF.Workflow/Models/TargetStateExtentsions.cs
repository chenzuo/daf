using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Workflow.Models
{
    public static class TargetStateExtentsions
    {
        public static DateTime OperationTime(this TargetState tstate)
        {
            switch (tstate.StateStatus)
            {
                case StateStatus.Responsed:
                    return tstate.ResponseTime.Value;
                case StateStatus.Planned:
                    return tstate.PlanTreatTime.Value;
                default:
                    return tstate.OperateTime.Value;
            }
        }

        public static string UserId(this TargetState tstate)
        {
            switch (tstate.StateStatus)
            {
                case StateStatus.Responsed:
                    return tstate.ResponsorId;
                case StateStatus.Planned:
                    return tstate.PlannerId;
                default:
                    return tstate.OperatorId;
            }
        }

        public static string UserName(this TargetState tstate)
        {
            switch (tstate.StateStatus)
            {
                case StateStatus.Responsed:
                    return tstate.ResponsorName;
                case StateStatus.Planned:
                    return tstate.PlannerName;
                default:
                    return tstate.OperatorName;
            }
        }
    }
}
