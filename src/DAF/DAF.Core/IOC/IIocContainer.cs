using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.IOC
{
    public interface IIocContainer : IIocContext
    {
        void BeginWorkUnitScope();
    }
}
