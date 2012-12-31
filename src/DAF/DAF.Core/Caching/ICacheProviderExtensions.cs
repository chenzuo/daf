using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public static class ICacheProviderExtensions
    {
        public static void Add(this ICacheProvider cache, string key, object value, IEnumerable<string> dependentEntitySets = null)
        {
            cache.Add(key, value, dependentEntitySets, TimeSpan.Zero, DateTime.MaxValue);
        }
    }
}
