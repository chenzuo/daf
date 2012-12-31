using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Info
{
    [DataContract]
    public class ResponseInfo
    {
        [DataMember]
        public string TargetStateId { get; set; }

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public DateTime OperationTime { get; set; }
    }
}
