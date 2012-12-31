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
        Planned = 1,
        [EnumMember]
        Responsed = 2,
        [EnumMember]
        TreatedNormal = 3,
        [EnumMember]
        TreatedWarn = 4,
        [EnumMember]
        TreatedError = 5
    }
}
