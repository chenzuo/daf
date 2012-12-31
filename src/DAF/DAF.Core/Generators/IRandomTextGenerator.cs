using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Generators
{
    public interface IRandomTextGenerator
    {
        string Generate(string chars, int length);
    }
}
