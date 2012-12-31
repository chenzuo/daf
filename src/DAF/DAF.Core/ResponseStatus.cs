using System;
using System.Runtime.Serialization;

namespace DAF.Core
{
    [DataContract]
    public enum ResponseStatus
    {
        [EnumMember]
        Success = 0,
        [EnumMember]
        Failed = 1,
        [EnumMember]
        Exception = 5
    }
}
