using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    [KnownType(typeof(UserSession))]
    [KnownType(typeof(SimplePermission))]
    public class Session : ISession
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public string ClientBaseUrl { get; set; }

        [DataMember]
        public string TransferedFromClientId { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public string AccessToken { get; set; }

        [DataMember]
        public UserSession User { get; set; }

        [DataMember]
        public string[] Roles { get; set; }

        [DataMember]
        public SimplePermission[] Permissions { get; set; }

        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public string DeviceInfo { get; set; }

        [DataMember]
        public string Theme { get; set; }

        [DataMember]
        public string Skin { get; set; }

        [DataMember]
        public string Locale { get; set; }

        [DataMember]
        public double TimeZone { get; set; }
    }
}
