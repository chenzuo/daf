using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Search
{
    public interface ISearchProvider
    {
        void Initialize();
        IIndex CreateIndex(string typeName);
        ISearch CreateSearch(string typeName);
        void Close();
    }
}
