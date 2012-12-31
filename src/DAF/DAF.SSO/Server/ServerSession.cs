using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.SSO.Server
{
    [Table("sso_ServerSession")]
    public class ServerSession
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(128)]
        public string SessionId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string CientId { get; set; }
        [StringLength(50)]
        public string FromCientId { get; set; }
        [StringLength(50)]
        public string FromSessionId { get; set; }
        [StringLength(50)]
        public string DeviceId { get; set; }
        [StringLength(500)]
        public string DeviceInfo { get; set; }
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(50)]
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpiryTime { get; set; }
        public DateTime? LastAccessTime { get; set; }
    }
}
