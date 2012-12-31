using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAF.SSO
{
    [DataContract]
    public class UserSession
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public Sex? Sex { get; set; }
        [DataMember]
        public DateTime? Birthday { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public string Avatar { get; set; }
    }

    public static class IUserExtensions
    {
        public static UserSession ToUserSession(this IUser user)
        {
            if (user == null)
                return null;
            return new UserSession
            {
                UserId = user.UserId,
                Account = user.Account,
                FullName = user.FullName,
                NickName = user.NickName,
                Sex = user.Sex,
                Birthday = user.Birthday,
                Email = user.Email,
                Mobile = user.Mobile,
                Avatar = user.Avatar
            };
        }

        public static string Name(this UserSession session)
        {
            return string.IsNullOrEmpty(session.FullName) ? (string.IsNullOrEmpty(session.NickName) ? session.Account : session.NickName) : session.FullName;
        }

        public static string Name(this IUser user)
        {
            return string.IsNullOrEmpty(user.FullName) ? (string.IsNullOrEmpty(user.NickName) ? user.Account : user.NickName) : user.FullName;
        }
    }
}
