using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Caching
{
    public class StaticShareCacheProvider : DictionaryCacheProviderBase
    {
        public StaticShareCacheProvider()
            : base(new Dictionary<string, object>())
        {
        }
    }

}
