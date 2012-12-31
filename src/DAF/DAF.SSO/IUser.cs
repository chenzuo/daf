using System;
using DAF.Core;

namespace DAF.SSO
{
    public interface IUser : IEntityWithStatus
    {
        string UserId { get; set; }

        string Account { get; set; }
        Sex? Sex { get; set; }
        DateTime? Birthday { get; set; }
        string Email { get; set; }
        string Mobile { get; set; }
        string Password { get; set; }

        string FullName { get; set; }
        string NickName { get; set; }
        string Avatar { get; set; }
    }
}
