﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DAF.Core.Data;
using DAF.Workflow.Models;

namespace DAF.Workflow.Messages
{
    public class TargetStateChangedMessage
    {
        public IEnumerable<TargetState> OldTargetStates { get; set; }
        public TargetState NewTargetState { get; set; }
        public DataOperation DataOperation { get; set; }

        public string OperationId { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime OperateTime { get; set; }
    }
}
