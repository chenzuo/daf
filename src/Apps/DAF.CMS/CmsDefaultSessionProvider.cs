using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using DAF.Core;
using DAF.SSO;
using DAF.SSO.Client;

namespace DAF.CMS
{
    public class CmsDefaultSessionProvider : IDefaultSessionProvider
    {
        public ISession NewSession()
        {
            var session = new Session
            {
                Theme = CmsHelper.CurrentSite.DefaultTheme.DefaultIfNull("Default"),
                Skin = CmsHelper.CurrentSite.DefaultSkin.DefaultIfNull("Default"),
                Locale = Thread.CurrentThread.CurrentCulture.Name,
                TimeZone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours,
                SessionId = HttpContext.Current.Session.SessionID
            };
            return session;
        }
    }
}
