using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    public class TransferSessionInfo
    {
        [DataMember]
        public string FromClientId { get; set; }
        [DataMember]
        public string FromSessionId { get; set; }
        [DataMember]
        public string ToClientId { get; set; }
        [DataMember]
        public string TransferToken { get; set; }
    }
}

