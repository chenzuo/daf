using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    public class SessionInfo
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string AccessToken { get; set; }
    }
}
