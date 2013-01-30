using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public class DictionaryCacheProvider : DictionaryCacheProviderBase
    {
        protected IDictionary<string, object> items;

        public DictionaryCacheProvider(IDictionary<string, object> items)
        {
            this.items = items;
        }

        protected override IEnumerable<string> GetAllKeys()
        {
            return items.Keys;
        }

        protected override void Add(string key, object value)
        {
            items.Add(key, value);
        }

        public override bool Contains(string key)
        {
            return items.ContainsKey(key);
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override void Clear()
        {
            items.Clear();
        }

        public override object GetData(string key)
        {
            return items[key];
        }

        public override void Remove(string key)
        {
            items.Remove(key);
        }

        public override void Dispose()
        {
            base.Dispose();
            items = null;
        }
    }
}
