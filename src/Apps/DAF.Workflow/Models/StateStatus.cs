using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Workflow.Models
{
    [DataContract]
    public enum StateStatus
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Started = 1,
        [EnumMember]
        Planned = 2,
        [EnumMember]
        Responsed = 3,
        [EnumMember]
        TreatedError = 10,
        [EnumMember]
        TreatedWarn = 11,
        [EnumMember]
        TreatedNormal = 19,
        [EnumMember]
        Finished = 99
    }
}
