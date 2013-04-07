using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.Routing;
using DAF.Core;

namespace DAF.Web
{
    public static class UriExtensions
    {
        public static string BaseUrl(this Uri url)
        {
            return string.Format("{0}://{1}", url.Scheme, url.Authority);
        }

        public static Uri AppendQueryString(this Uri url, object paras)
        {
            return AppendQueryString(url, new RouteValueDictionary(paras));
        }

        public static Uri AppendQueryString(this Uri url, RouteValueDictionary paras)
        {
            Dictionary<string, string> dic = new Dictionary<string,string>();
            if(!string.IsNullOrEmpty(url.Query))
               dic = url.Query.ToDictionary(o => o.ToLower(), o => o);
            foreach (var key in paras.Keys)
            {
                var name = key.ToLower();
                var value = (paras[key] ?? "").ToString();
                if (dic.ContainsKey(name))
                    dic[name] = value;
                else
                    dic.Add(name, HttpUtility.UrlEncode(value));
            }

            UriBuilder ub = new UriBuilder(url);
            ub.Query = dic.ToFormatString();
            return ub.Uri;
        }
    }
}
