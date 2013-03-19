using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core
{
    public interface IObjectProvider<T>
    {
        T GetObject();
        void SaveObject(T obj);
    }
}
