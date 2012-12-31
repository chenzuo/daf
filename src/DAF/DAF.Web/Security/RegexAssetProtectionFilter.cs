using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DAF.Core;

namespace DAF.Web.Security
{
    public class RegexAssetProtectionFilter : IAssetProtectionFilter
    {
        private string regex = null;
        private bool allowedIfMatched = true;

        public RegexAssetProtectionFilter(string regex, bool allowedIfMatched)
        {
            this.regex = regex;
            this.allowedIfMatched = allowedIfMatched;
        }

        public bool AllowAccess(HttpContext context)
        {
            Uri url = context.Request.UrlReferrer;
            if (url == null)
                return true;

            if (string.IsNullOrEmpty(regex))
                return true;

            if (Regex.IsMatch(url.AbsoluteUri, regex, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                return allowedIfMatched;
            return !allowedIfMatched;
        }
    }
}
