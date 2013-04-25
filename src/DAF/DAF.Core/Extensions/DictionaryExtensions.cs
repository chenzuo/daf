using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace DAF.Core
{
    public static class DictionaryExtensions
    {
        public static V GetValue<T, V>(this IDictionary<T, V> dic, T key, V defaultValue)
        {
            if (dic == null || !dic.ContainsKey(key))
                return defaultValue;
            return dic[key];
        }

        public static IDictionary<T, V> Add<T, V>(this IDictionary<T, V> dic, IEnumerable<KeyValuePair<T, V>> values, bool replaceWhenExists = true)
        {
            if (values != null)
            {
                foreach (var kvp in values)
                {
                    if (dic.ContainsKey(kvp.Key))
                    {
                        if (replaceWhenExists)
                            dic[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        dic.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            return dic;
        }

        //public static Dictionary<T, V> Add<T, V>(this Dictionary<T, V> dic, object values)
        //{
        //    if (values != null)
        //    {
        //        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
        //        {
        //            object obj2 = descriptor.GetValue(values);
        //            dic.Add(descriptor.Name, obj2);
        //        }
        //    }
        //    return dic;
        //}

        public static string ToFormatString<T, V>(this IDictionary<T, V> dic, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "", Func<T, string> keyToString = null, Func<V, string> valToString = null)
        {
            if (dic == null || dic.Count <= 0)
                return string.Empty;
            if (keyToString == null)
                keyToString = o => o.ToString();
            if (valToString == null)
                valToString = o => o == null ? null : o.ToString();
            StringBuilder sb = new StringBuilder();
            int idx =0;
            foreach (T key in dic.Keys)
            {
                sb.AppendFormat("{0}{1}{2}{3}{4}", keyToString(key), equal, leftComma, valToString(dic[key]), rightComma);
                if(idx < dic.Count - 1)
                    sb.Append(comma);
                idx ++;
            }
            return sb.ToString();
        }

        public static NameValueCollection ToNameValueCollection<T, V>(this IDictionary<T,V> dic, Func<T, string> keyToString = null, Func<V, string> valToString = null)
        {
            if (dic == null)
                return null;
            if (keyToString == null)
                keyToString = o => o.ToString();
            if (valToString == null)
                valToString = o => o == null ? null : o.ToString();
            NameValueCollection nv = new NameValueCollection();
            foreach (var kvp in dic)
            {
                nv.Add(keyToString(kvp.Key), valToString(kvp.Value));
            }
            return nv;
        }
    }
}
