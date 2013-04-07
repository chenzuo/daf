using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace DAF.SSO.Client
{
    public class DefaultSessionProvider : IDefaultSessionProvider
    {
        public ISession NewSession()
        {
            var session = new Session
            {
                Theme = "Default",
                Skin = "Default",
                Locale = Thread.CurrentThread.CurrentCulture.Name,
                TimeZone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours,
                SessionId = HttpContext.Current.Session.SessionID
            };
            return session;
        }
    }
}
