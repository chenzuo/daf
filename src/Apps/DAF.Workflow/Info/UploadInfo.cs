using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Info
{
    [DataContract]
    public class UploadInfo
    {
        [DataMember]
        public string TargetId { get; set; }
        [DataMember]
        public string TargetStateId { get; set; }
        [DataMember]
        public string UploadId { get; set; }
        [DataMember]
        public string FileUrl { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public bool Removed { get; set; }
        [DataMember]
        public bool? Verified { get; set; }

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public DateTime OperationTime { get; set; }
    }
}
