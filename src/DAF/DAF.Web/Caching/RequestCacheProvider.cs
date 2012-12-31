using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core.Caching;

namespace DAF.Web.Caching
{
    public class RequestCacheProvider : DictionaryCacheProviderBase
    {
        public RequestCacheProvider()
            : base(HttpContext.Current == null ? new Dictionary<string, object>() : HttpContext.Current.Items)
        {
        }
    }
}
