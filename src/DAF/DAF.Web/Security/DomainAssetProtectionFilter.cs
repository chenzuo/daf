using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;

namespace DAF.Web.Security
{
    public class DomainAssetProtectionFilter : IAssetProtectionFilter
    {
        private string[] allowDomains;
        private string[] forbiddenDomains;

        public DomainAssetProtectionFilter(string allowDomains, string forbiddenDomains)
        {
            this.allowDomains = allowDomains.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.forbiddenDomains = forbiddenDomains.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public bool AllowAccess(HttpContext context)
        {
            Uri url = context.Request.UrlReferrer;
            string domain = url.BaseUrl().ToLower();
            if (url == null || domain == context.Request.Url.BaseUrl().ToLower())
                return true;

            bool allowed = true;
            if (allowDomains != null && allowDomains.Length > 0)
                allowed = allowDomains.Any(o => domain.StartsWith(o));
            if (forbiddenDomains != null && forbiddenDomains.Length > 0)
                allowed = !forbiddenDomains.Any(o => domain.StartsWith(o));
            return allowed;
        }
    }
}
