using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DAF.SSO.Client
{
    public class TransferSignOnInfo
    {
        [DataMember]
        [Required]
        public string ClientId { get; set; }
        [DataMember]
        [Required]
        public string DeviceId { get; set; }
        [DataMember]
        public string DeviceInfo { get; set; }
        [DataMember]
        [Required]
        public string SessionId { get; set; }
        [DataMember]
        [Required]
        public string FromClientId { get; set; }
        [DataMember]
        [Required]
        public string FromSessionId { get; set; }
    }
}
