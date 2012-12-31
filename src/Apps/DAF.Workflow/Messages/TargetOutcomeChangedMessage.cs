using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DAF.Core.Data;
using DAF.Workflow.Models;

namespace DAF.Workflow.Messages
{
    public class TargetOutcomeChangedMessage
    {
        public TargetOutcome TargetOutcome { get; set; }
        public DataOperation DataOperation { get; set; }

        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime OperateTime { get; set; }
    }
}
