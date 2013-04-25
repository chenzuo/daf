using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.IOC
{
    public interface IIocModule
    {
        void Load(IIocBuilder builder);
    }
}
