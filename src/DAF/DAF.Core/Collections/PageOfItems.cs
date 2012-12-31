using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAF.Collections
{
    public class PageOfItems<T> : List<T>, IPageOfItems<T>
    {
        public PageOfItems(IQueryable<T> query)
            : this(query, 0, 20)
        {
        }

        public PageOfItems(IQueryable<T> query, int pageNumber, int pageSize)
        {
            int totalItemCount = query.Count();
            if (pageNumber < 0)
                pageNumber = 0;

            if (pageNumber > 1 && totalItemCount <= pageSize)
                pageNumber = 1;

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            AddRange(query.ToList());

            PagingInfo = new PagingInfo(pageNumber, pageSize, totalItemCount);
        }

        public PagingInfo PagingInfo { get; set; }
    }
}
