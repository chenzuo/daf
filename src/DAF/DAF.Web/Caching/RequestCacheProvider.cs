using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core.Caching;

namespace DAF.Web.Caching
{
    public class RequestCacheProvider : SimpleDictionaryCacheProvider
    {
        public RequestCacheProvider()
            : base(HttpContext.Current.Items)
        {
        }
    }
}
