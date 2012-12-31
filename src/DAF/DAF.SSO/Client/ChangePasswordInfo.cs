using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DAF.SSO.Client
{
    [DataContract]
    public class ChangePasswordInfo
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
        public string UserId { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataMember]
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataMember]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
