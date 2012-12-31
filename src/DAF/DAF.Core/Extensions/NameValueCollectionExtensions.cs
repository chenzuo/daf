using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection nv)
        {
            return ToDictionary<string, string>(nv, o => o, o => o);
        }

        public static Dictionary<T, V> ToDictionary<T, V>(this NameValueCollection nv, Func<string, T> keyResover, Func<string, V> valResolver)
        {
            if (nv == null)
                return null;
            Dictionary<T, V> dic = new Dictionary<T, V>();
            foreach (var key in nv.AllKeys)
                dic.Add(keyResover(key), valResolver(nv[key]));
            return dic;
        }

        public static string ToFormatString(this NameValueCollection nv, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "", Func<string, string> keyToString = null, Func<string, string> valToString = null)
        {
            if (nv == null || nv.Count <= 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            int idx = 0;
            if (keyToString == null)
                keyToString = o => o;
            if (valToString == null)
                valToString = o => o;
            foreach (string key in nv.AllKeys)
            {
                string val = nv[key];
                sb.AppendFormat("{0}{1}{2}{3}{4}", keyToString(key), equal, leftComma, valToString(val), rightComma);
                if (idx < nv.Count - 1)
                    sb.Append(comma);
                idx++;
            }
            return sb.ToString();
        }
    }
}
