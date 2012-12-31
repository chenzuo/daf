using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DAF.SSO.Client
{
    [DataContract]
    public class SignOnInfo
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
        public string AccountOrEmailOrMobile { get; set; }
        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
