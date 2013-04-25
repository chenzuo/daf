using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.IOC
{
    public interface IAutoRegister
    {
        void Register(IIocBuilder builder, Type type);
    }
}
