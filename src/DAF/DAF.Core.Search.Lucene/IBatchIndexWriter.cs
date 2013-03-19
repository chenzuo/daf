using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Search.Lucene
{
    public interface IBatchIndexWriter
    {
        void Run();
    }
}
