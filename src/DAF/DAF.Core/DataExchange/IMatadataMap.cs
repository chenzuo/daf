using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.DataExchange
{
    public interface IMatadataMap
    {
        bool IsMapDefined(Type from, Type to);
        object Map(Type from, Type to, object obj);
    }

    public static class IMatadataMapExtensions
    {
        public static bool IsMapDefined<TF, TT>(this IMatadataMap map)
        {
            return map.IsMapDefined(typeof(TF), typeof(TT));
        }

        public static TT Map<TF, TT>(this IMatadataMap map, TF obj)
        {
            return (TT)map.Map(typeof(TF), typeof(TT), obj);
        }
    }
}
