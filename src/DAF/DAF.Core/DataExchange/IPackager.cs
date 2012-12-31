using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.DataExchange
{
    public interface IPackager
    {
        Type Unpackage(string zipFilePath);
        string Package(Type data);
    }
}
