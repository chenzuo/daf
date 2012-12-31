using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DAF.Workflow.Models;

namespace DAF.Workflow.Messages
{
    public class NextTargetFlowCreatedMessage
    {
        public TargetFlow FinishedTargetFlow { get; set; }
        public TargetFlow CreatedTargetFlow { get; set; }

        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime OperateTime { get; set; }
    }
}
