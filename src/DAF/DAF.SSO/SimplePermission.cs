using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    [KnownType(typeof(PermissionType))]
    public class SimplePermission
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public PermissionType PermissionType { get; set; }
        [DataMember]
        public string[] ProtectedUris { get; set; }
        [DataMember]
        public string[] AllowedUris { get; set; }
    }
}
