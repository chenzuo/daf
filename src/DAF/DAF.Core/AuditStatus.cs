using System;
using System.Runtime.Serialization;

namespace DAF.Core
{
    [DataContract]
    public enum AuditStatus
    {
        [EnumMember]
        Draft = 0,
        [EnumMember]
        Submitted = 1,
        [EnumMember]
        Approval = 2,
        [EnumMember]
        Audited = 3,
        [EnumMember]
        Published = 4
    }
}
