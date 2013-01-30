using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAF.Core.Caching
{
    public class ThreadSlotCacheProvider : DictionaryCacheProviderBase
    {
        private const string CacheSlotName = "ThreadSlotCacheData";

        private ConcurrentDictionary<string, object> GetCachedData()
        {
            var slot = Thread.GetNamedDataSlot(CacheSlotName);
            if (slot == null)
            {
                slot = Thread.AllocateNamedDataSlot(CacheSlotName);
            }
            var dic = Thread.GetData(slot) as ConcurrentDictionary<string, object>;
            if (dic == null)
            {
                dic = new ConcurrentDictionary<string, object>();
                Thread.SetData(slot, dic);
            }
            return dic;
        }

        protected override IEnumerable<string> GetAllKeys()
        {
            var dic = GetCachedData();
            return dic.Keys;
        }

        protected override void Add(string key, object value)
        {
            var dic = GetCachedData();
            dic.AddOrUpdate(key, value, (k, o) => o);
        }

        public override bool Contains(string key)
        {
            var dic = GetCachedData();
            return dic.ContainsKey(key);
        }

        public override int Count
        {
            get 
            {
                var dic = GetCachedData();
                return dic.Count;
            }
        }

        public override void Clear()
        {
            var dic = GetCachedData();
            dic.Clear();
        }

        public override object GetData(string key)
        {
            var dic = GetCachedData();
            return dic[key];
        }

        public override void Remove(string key)
        {
            var dic = GetCachedData();
            object obj = null;
            if (!dic.TryRemove(key, out obj))
            {
                dic[key] = null;
            }
        }

        public override void Dispose()
        {
            Thread.FreeNamedDataSlot(CacheSlotName);
        }
    }
}
