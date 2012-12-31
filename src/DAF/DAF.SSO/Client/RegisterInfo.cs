using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DAF.SSO.Server;

namespace DAF.SSO.Client
{
    [DataContract]
    [KnownType(typeof(Sex))]
    public class RegisterInfo
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
        public string Account { get; set; }
        [DataMember]
        [StringLength(50)]
        public string FullName { get; set; }
        [DataMember]
        [StringLength(50)]
        public string NickName { get; set; }
        [DataMember]
        public Sex? Sex { get; set; }
        [DataMember]
        public DateTime? Birthday { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Email { get; set; }
        [DataMember]
        [StringLength(50)]
        public string Mobile { get; set; }

        [DataMember]
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
