using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public abstract class DictionaryCacheProviderBase : ICacheProvider
    {
        protected IDictionary items;
        protected string dependencySpliter = "$depends$";

        public DictionaryCacheProviderBase(IDictionary items)
        {
            this.items = items;
        }

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

        protected IEnumerable<string> GetAllKeys()
        {
            List<string> keys = new List<string>();
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            return keys;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Add(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            key = BuildCacheKey(key, dependentEntitySets);
            items[key] = value;
        }

        public virtual bool Contains(string key)
        {
            foreach (var k in items.Keys)
            {
                var sk = k.ToString();
                if (KeyEquals(sk, key))
                    return true;
            }
            return false;
        }

        public virtual int Count
        {
            get { return items.Count; }
        }

        public virtual void Clear()
        {
            items.Clear();
        }

        public virtual object GetData(string key)
        {
            foreach (var k in items.Keys)
            {
                var sk = k.ToString();
                if (KeyEquals(sk, key))
                    return items[sk];
            }
            return null;
        }

        public virtual void Remove(string key)
        {
            GetAllKeys().Where(o => KeyEquals(o, key)).ForEach(o => items.Remove(o));
        }

        public virtual void RemoveDependencySet(IEnumerable<string> sets)
        {
            if (sets == null || sets.Count() <= 0)
                return;

            GetAllKeys().Where(o => KeyDepends(o, sets)).ForEach(o => items.Remove(o));
        }

        public virtual object this[string key]
        {
            get { return GetData(key); }
        }
    }
}
