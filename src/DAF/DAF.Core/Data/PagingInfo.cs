using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data
{
    public class PagingInfo
    {
        public PagingInfo()
        {
        }

        public PagingInfo(int pageNumber, int pageSize, int totalItemCount)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }

        public int TotalPageCount
        {
            get { return (int)Math.Ceiling((double)TotalItemCount / PageSize); }
        }
        public int StartPosition
        {
            get { return (PageIndex - 1) * PageSize + 1; }
        }
        public int EndPosition
        {
            get { return PageIndex * PageSize > TotalItemCount ? TotalItemCount : PageIndex * PageSize; }
        }
    }
}
