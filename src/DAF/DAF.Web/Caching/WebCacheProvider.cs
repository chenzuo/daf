using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using DAF.Core.Caching;

namespace DAF.Web.Caching
{
    public class WebCacheProvider : ICacheProvider
    {
        private string dependentEntitySetPrefix = "dependent_entity_set_";
        private string globalCacheDependentKey = "globle_dependent_key";
        private Cache cache = HttpContext.Current == null ? HttpRuntime.Cache : HttpContext.Current.Cache;

        public WebCacheProvider()
        {
            EnsureDependencySet(globalCacheDependentKey);
        }

        #region ICacheService Members

        public void Initialize()
        {
        }

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        /// <param name="dependentEntitySets">The list of dependent entity sets.</param>
        /// <param name="slidingExpiration">The sliding expiration - IGNORED.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public void Add(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            EnsureGlobalDependency();
            if (dependentEntitySets == null)
                dependentEntitySets = Enumerable.Empty<string>();

            var dependencies = dependentEntitySets.Select(c => dependentEntitySetPrefix + c);

            foreach (var entitySet in dependencies)
            {
                this.EnsureDependencySet(entitySet);
            }

            try
            {
                CacheDependency cd = new CacheDependency(null, dependencies.Concat(new string[] { globalCacheDependentKey }).ToArray());
                cache.Insert(key, value, cd, absoluteExpiration, slidingExpiration, CacheItemPriority.Normal, null);
            }
            catch (Exception)
            {
                // there's a possibility that one of the dependencies has been evicted by another thread
                // in this case just don't put this item in the cache
            }
        }

        public bool Contains(string key)
        {
            return GetData(key) != null;
        }

        public int Count
        {
            get 
            {
                List<string> keys = new List<string>();
                var enumerator = cache.GetEnumerator();
                int keyCount = 0;
                while (enumerator.MoveNext())
                {
                    string key =enumerator.Key.ToString() ;
                    if (key == globalCacheDependentKey || key.StartsWith(dependentEntitySetPrefix))
                        keyCount++;
                }

                return cache.Count - keyCount; 
            }
        }

        public void Clear()
        {
            cache.Remove(globalCacheDependentKey);
        }

        public object GetData(string key)
        {
            return cache.Get(key);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void RemoveDependencySet(IEnumerable<string> sets)
        {
            foreach (string entitySet in sets)
            {
                cache.Remove(dependentEntitySetPrefix + entitySet);
            }
        }

        public object this[string key]
        {
            get { return GetData(key); }
        }

        #endregion

        private void EnsureGlobalDependency()
        {
            if (cache.Get(globalCacheDependentKey) == null)
            {
                try
                {
                    cache.Insert(globalCacheDependentKey, globalCacheDependentKey, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                }
                catch (Exception)
                {
                    // ignore exceptions.
                }
            }
        }

        private void EnsureDependencySet(string key)
        {
            if (cache.Get(key) == null)
            {
                try
                {
                    var globalDependency = new CacheDependency(null, new string[] { globalCacheDependentKey });
                    cache.Insert(key, key, globalDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                }
                catch (Exception)
                {
                    // ignore exceptions.
                }
            }
        }
    }
}
