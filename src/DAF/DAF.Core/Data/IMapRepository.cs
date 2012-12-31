using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAF.Core.Data
{
    public interface IMapRepository<U, T> : IRepository<T>
        where T : class
        where U : class, T
    {
    }
}
