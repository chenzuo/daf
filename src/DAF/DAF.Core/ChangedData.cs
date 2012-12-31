using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public class ChangedData<T>
    {
        public T[] NewItems { get; set; }
        public T[] ModifiedItems { get; set; }
        public T[] DeletedItems { get; set; }
    }
}
