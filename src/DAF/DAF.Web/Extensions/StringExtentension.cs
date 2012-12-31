using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace DAF.Web
{
    public static class StringExtentension
    {
        public static string AppendQueryString(this string url, RouteValueDictionary paras)
        {
            string split = url.IndexOf('?') > 0 ? "&" : "?";
            string qs = string.Empty;
            foreach (var key in paras.Keys)
            {
                qs += string.Format("{0}={1}&", key, paras[key]);
            }
            return string.Concat(url, split, qs);
        }

        public static string AppendQueryString(this string url, object paras)
        {
            return AppendQueryString(url, new RouteValueDictionary(paras));
        }
    }
}
