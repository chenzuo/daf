using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DAF.Web.Mvc
{
    public static class ViewDataDictionaryExtensions
    {
        public static IDictionary<string, object> Extract(this ViewDataDictionary dic, Func<string, bool> predicate)
        {
            if (dic == null)
                return null;
            ViewDataDictionary dic2 = new ViewDataDictionary();
            foreach (var k in dic.Keys)
            {
                if (predicate(k))
                {
                    dic2.Add(k.Replace('_', '-'), dic[k]);
                }
            }
            return dic2;
        }
    }
}
