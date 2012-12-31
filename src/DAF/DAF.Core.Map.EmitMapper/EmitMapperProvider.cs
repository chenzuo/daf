using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;
using DAF.Core.Map;

namespace DAF.Core.Map.EmitMapper
{
    public class EmitMapperProvider : IMapProvider
    {
        public TT Map<TF, TT>(TF obj, Func<TT> existsingObj)
        {
            var map = ObjectMapperManager.DefaultInstance.GetMapper<TF, TT>();
            return map.Map(obj, existsingObj());
        }
    }
}
