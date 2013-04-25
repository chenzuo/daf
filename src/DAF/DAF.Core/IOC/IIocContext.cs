using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.IOC
{
    public interface IIocContext
    {
        bool IsRegistered<T>(string name = null);
        T Resolve<T>(string name = null);
        T ResolveOptional<T>(string name = null) where T : class;
        IEnumerable<T> ResolveAll<T>();
    }
}
