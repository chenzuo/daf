using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public class SimpleDictionaryCacheProvider : DictionaryCacheProviderBase
    {
        protected IDictionary items;

        public SimpleDictionaryCacheProvider(IDictionary items)
        {
            this.items = items;
        }

        protected override IEnumerable<string> GetAllKeys()
        {
            return items.Keys.Cast<string>();
        }

        protected override void Add(string key, object value)
        {
            items[key] = value;
        }

        public override bool Contains(string key)
        {
            foreach (var k in items.Keys)
            {
                var sk = k.ToString();
                if (KeyEquals(sk, key))
                    return true;
            }
            return false;
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
            foreach (var k in items.Keys)
            {
                var sk = k.ToString();
                if (KeyEquals(sk, key))
                    return items[sk];
            }
            return null;
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
