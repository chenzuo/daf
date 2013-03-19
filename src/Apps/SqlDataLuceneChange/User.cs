using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Search;
using DAF.SSO;

namespace SqlDataLuceneChange
{
    [Table("sso_User")]
    [Document]
    public class User : IEntityWithStatus, IUser
    {
        public User()
        {
        }

        [Key]
        [Required]
        [StringLength(50)]
        [Field(0, IsKey = true, IndexMode = FieldIndexMode.NO)]
        public string UserId { get; set; }
        private string account;
        [Required]
        [StringLength(50)]
        [Field(1)]
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
        [Field(2)]
        public string NickName { get; set; }
        [StringLength(50)]
        [Field(3)]
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
    }
}
