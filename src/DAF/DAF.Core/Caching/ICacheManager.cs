using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public interface ICacheManager
    {
        ICacheProvider CreateCacheProvider(CacheScope cacheScope);
    }
}
