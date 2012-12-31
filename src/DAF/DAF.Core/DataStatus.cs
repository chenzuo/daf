using System;
using System.Runtime.Serialization;

namespace DAF.Core
{
    [DataContract]
    public enum DataStatus
    {
        [EnumMember]
        Normal = 1,
        [EnumMember]
        ReadOnly = 2,
        [EnumMember]
        Locked = 3,
        [EnumMember]
        Deleted = 99
    }
}
