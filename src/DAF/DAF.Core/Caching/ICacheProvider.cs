using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public interface ICacheProvider
    {
        /// <summary>
        /// initialize the cache manager with the specified name
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        /// <param name="dependentEntitySets">The list of dependent entity sets.</param>
        /// <param name="slidingExpiration">The sliding expiration - IGNORED.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        void Add(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration);

        /// <summary>
        /// Returns true if key refers to item current stored in cache
        /// </summary>
        /// <param name="key">Key of item to check for</param>
        /// <returns>True if item referenced by key is in the cache</returns>
        bool Contains(string key);

        /// <summary>
        /// Returns the number of items currently in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
        /// </summary>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        void Clear();

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        /// <param name="key">Key of item to return from cache.</param>
        /// <returns>Value stored in cache</returns>
        /// <exception cref="ArgumentNullException">Provided key is null</exception>
        /// <exception cref="ArgumentException">Provided key is an empty string</exception>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        object GetData(string key);

        /// <summary>
        /// Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        /// <param name="key">Key of item to remove from cache.</param>
        /// <exception cref="ArgumentNullException">Provided key is null</exception>
        /// <exception cref="ArgumentException">Provided key is an empty string</exception>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        void Remove(string key);

        /// <summary>
        /// Remove the dependency cache sets.
        /// </summary>
        /// <param name="sets">the dependency cache sets</param>
        void RemoveDependencySet(IEnumerable<string> sets);

        /// <summary>
        /// Returns the item identified by the provided key
        /// </summary>
        /// <param name="key">Key to retrieve from cache</param>
        /// <exception cref="ArgumentNullException">Provided key is null</exception>
        /// <exception cref="ArgumentException">Provided key is an empty string</exception>
        /// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the cache items.
        /// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
        object this[string key] { get; }
    }

}
