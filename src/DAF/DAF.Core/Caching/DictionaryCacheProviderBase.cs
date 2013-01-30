using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public abstract class DictionaryCacheProviderBase : ICacheProvider
    {
        protected string dependencySpliter = "$depends$";

        protected string BuildCacheKey(string key, IEnumerable<string> dependentEntitySets)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(key);
            if (dependentEntitySets != null)
            {
                sb.Append(dependencySpliter);
                foreach (var d in dependentEntitySets)
                    sb.AppendFormat("[{0}]", d);
            }
            return sb.ToString();
        }

        protected bool KeyEquals(string key1, string key2)
        {
            return key1 == key2 || key1.StartsWith(key2 + dependencySpliter);
        }

        protected bool KeyDepends(string key, IEnumerable<string> sets)
        {
            return key.IndexOf(dependencySpliter) > 0 && sets.Any(o => key.IndexOf(string.Concat("[", o, "]")) > 0);
        }

        protected abstract IEnumerable<string> GetAllKeys();
        protected abstract void Add(string key, object value);

        public virtual void Initialize()
        {
        }

        public virtual void Add(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            key = BuildCacheKey(key, dependentEntitySets);
            Add(key, value);
        }

        public abstract bool Contains(string key);

        public abstract int Count { get; }

        public abstract void Clear();

        public abstract object GetData(string key);

        public abstract void Remove(string key);

        public virtual void RemoveDependencySet(IEnumerable<string> sets)
        {
            if (sets == null || sets.Count() <= 0)
                return;

            GetAllKeys().Where(o => KeyDepends(o, sets)).ForEach(o => Remove(o));
        }

        public virtual object this[string key]
        {
            get { return GetData(key); }
        }

        public virtual void Dispose()
        {
            Clear();
        }
    }
}
