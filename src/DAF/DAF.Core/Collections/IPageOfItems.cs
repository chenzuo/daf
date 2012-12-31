using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Collections
{
    public interface IPageOfItems<out T> : IEnumerable<T>
    {
        PagingInfo PagingInfo { get; set; }
    }
}
