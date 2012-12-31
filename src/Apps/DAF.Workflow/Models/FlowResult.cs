using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Workflow.Models
{
    [DataContract]
    public enum FlowResult
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Yes = 1,
        [EnumMember]
        No = 2,
    }
}
