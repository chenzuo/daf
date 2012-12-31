using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Models
{
    [DataContract]
    public enum FileStatus
    {
        [EnumMember]
        Draft = 1,
        [EnumMember]
        Submitted = 2,
        [EnumMember]
        Verified = 3,
        [EnumMember]
        InValid = 99
    }
}
