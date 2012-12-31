using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    public enum Sex
    {
        [EnumMember]
        Female = 0,
        [EnumMember]
        Male = 1,
    }
}
