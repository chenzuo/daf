using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Search
{
    public interface IHit
    {
        object GetValue(string fieldName);
        double Score { get; set; }
    }
}
