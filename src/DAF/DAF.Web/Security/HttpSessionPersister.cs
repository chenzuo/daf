using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Security;

namespace DAF.Web.Security
{
    public class HttpSessionPersister : ISessionPersister
    {
        #region ISessionPersister Members

        public ISession GetCurrentSession()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                return HttpContext.Current.Session["CurrentSession"] as ISession;
            return null;
        }

        public void SaveSession(ISession session)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session["CurrentSession"] = session;
        }

        #endregion
    }
}
