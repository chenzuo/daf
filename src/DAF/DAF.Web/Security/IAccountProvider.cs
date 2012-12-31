using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Web.Security
{
    public interface IAccountProvider
    {
        LoginMessage Login(string account, string password, string sessionId, string device, string deviceId);
        void Logout();
        string AuthUrl { get; }
    }
}
