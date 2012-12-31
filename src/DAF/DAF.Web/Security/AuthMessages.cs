using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Web.Security
{
    public class LoginMessage
    {
        public LoginStatus Status { get; set; }
        public string Message { get; set; }
        public string EncryptedSession { get; set; }
    }

}
