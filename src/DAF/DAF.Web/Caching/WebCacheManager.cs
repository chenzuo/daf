using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core.Caching;

namespace DAF.Web.Caching
{
    public class WebCacheManager : ICacheManager
    {
        public WebCacheManager()
        {
        }

        public ICacheProvider CreateCacheProvider(CacheScope scope)
        {
            switch (scope)
            {
                case CacheScope.WorkUnit:
                    return new RequestCacheProvider();
                case CacheScope.Global:
                default:
                    return new WebCacheProvider();
            }
        }
    }
}
