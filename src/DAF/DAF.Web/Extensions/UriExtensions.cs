using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace DAF.Web
{
    public static class UriExtensions
    {
        public static string BaseUrl(this Uri url)
        {
            return string.Format("{0}://{1}", url.Scheme, url.Authority);
        }
    }
}
