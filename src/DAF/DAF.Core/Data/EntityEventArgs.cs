using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data
{
    public class EntityEventArgs<T> : System.EventArgs where T : class
    {
        public T OriginalEntity { get; set; }
        public T Entity { get; set; }
        public DataOperation Action { get; set; }
        public IRepository<T> Repository { get; set; }
    }

    public delegate void EntityEventHandler<TEntity>(EntityEventArgs<TEntity> args) where TEntity : class;
}
