using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Security;

namespace DAF.SSO.Server
{
    [Table("sso_User")]
    public class User : IEntityWithStatus, IUser
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }

        [Key]
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }
        private string account;
        [Required]
        [StringLength(50)]
        public string Account
        {
            get { return account; }
            set { account = value.ToLower(); }
        }
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        public string ClientId { get; set; }
        [StringLength(256)]
        public string SecurityCode { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        public Sex? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [StringLength(10)]
        public string Locale { get; set; }
        public double TimeZone { get; set; }
        [StringLength(20)]
        public string Theme { get; set; }
        [StringLength(20)]
        public string Skin { get; set; }
        [StringLength(200)]
        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }
        public DataStatus Status { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
