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
        PeriodBegin = 5,
        [EnumMember]
        PeriodStop = 6,
        [EnumMember]
        PeriodEnd = 7,
        [EnumMember]
        End = 9
    }
}
