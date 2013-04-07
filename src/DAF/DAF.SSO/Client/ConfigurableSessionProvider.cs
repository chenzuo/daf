using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Configuration;
using DAF.Core;

namespace DAF.SSO.Client
{
    public class ConfigurableSessionProvider : IDefaultSessionProvider
    {
        public ISession NewSession()
        {
            var session = new Session
            {
                Theme = ConfigurationManager.AppSettings["DefaultTheme"].DefaultIfNull("Default"),
                Skin = ConfigurationManager.AppSettings["DefaultSkin"].DefaultIfNull("Default"),
                Locale = Thread.CurrentThread.CurrentCulture.Name,
                TimeZone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours,
                SessionId = HttpContext.Current.Session.SessionID
            };
            return session;
        }
    }
}
