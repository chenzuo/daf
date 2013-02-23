using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Models
{
    [DataContract]
    public enum StateType
    {
        [EnumMember]
        Begin = 0,
        [EnumMember]
        Stop = 1,
        [EnumMember]
        PeriodBegin = 3,
        [EnumMember]
        PeriodStop = 4,
        [EnumMember]
        PeriodEnd = 5,
        [EnumMember]
        ParallelBegin = 6,
        [EnumMember]
        ParallelStop = 7,
        [EnumMember]
        ParallelEnd = 8,
        [EnumMember]
        End = 99
    }
}
