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

        #region ICacheManager Members

        public ICacheProvider CreateCacheProvider(CacheScope cacheScope)
        {
            switch (cacheScope)
            {
                case CacheScope.Global:
                    return new WebCacheProvider();
                case CacheScope.WorkUnit:
                    return new RequestCacheProvider();
            }

            return new WebCacheProvider();
        }

        #endregion
    }
}
