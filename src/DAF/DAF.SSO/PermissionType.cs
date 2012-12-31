using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    public enum PermissionType
    {
        [EnumMember]
        Operation = 1,
        [EnumMember]
        DataField = 2,
        [EnumMember]
        DataScope = 3
    }
}
