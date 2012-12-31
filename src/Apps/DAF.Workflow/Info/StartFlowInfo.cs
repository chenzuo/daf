using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Info
{
    [DataContract]
    public class StartFlowInfo
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string FlowCodeOrTargetType { get; set; }
        [DataMember]
        public string TargetId { get; set; }
        [DataMember]
        public string FlowTitle { get; set; }
        [DataMember]
        public string FlowMessage { get; set; }
        [DataMember]
        public string TargetFlowCode { get; set; }
        [DataMember]
        public string StartTitle { get; set; }
        [DataMember]
        public string StartMessage { get; set; }
        [DataMember]
        public string LastTargetFlowId { get; set; }

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public DateTime OperationTime { get; set; }
    }
}
