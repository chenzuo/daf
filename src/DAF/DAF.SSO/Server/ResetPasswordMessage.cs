using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.SSO.Server
{
    public class ResetPasswordMessage
    {
        public string ClientId { get; set; }
        public string Language { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NewPassword { get; set; }
        public string Account { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
    }
}
