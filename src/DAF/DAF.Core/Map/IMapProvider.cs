using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Map
{
    public interface IMapProvider
    {
        TT Map<TF, TT>(TF obj, Func<TT> existsingObj);
    }
}
