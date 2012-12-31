using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public class StaticShareCacheManager : ICacheManager
    {
        public ICacheProvider CreateCacheProvider(CacheScope cacheScope)
        {
            return new StaticShareCacheProvider();
        }
    }
}
