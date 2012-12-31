using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DAF.Web.Security
{
    public interface IAuthProvider
    {
        LoginMessage AuthenticateAccount(string site, string device, string deviceId, string sessionId, string account, string password);
        LoginMessage GetSession(string site, string sessionId);
        void ClearSession(string site, string sessionId, string token);
        void UpdateSessionTime(string site, string sessionId, string token);
    }
}
