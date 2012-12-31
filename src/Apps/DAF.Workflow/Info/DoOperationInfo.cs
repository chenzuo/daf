using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DAF.Workflow.Models;

namespace DAF.Workflow.Info
{
    [DataContract]
    public class DoOperationInfo
    {
        [DataMember]
        public string TargetFlowId { get; set; }
        [DataMember]
        public string TargetId { get; set; }
        [DataMember]
        public string TargetStateId { get; set; }
        [DataMember]
        public string OperationId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string NextStateIdOrCode { get; set; }

        [DataMember]
        public bool Planned { get; set; }

        [DataMember]
        public bool Cancelled { get; set; }

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public DateTime OperationTime { get; set; }
    }
}
