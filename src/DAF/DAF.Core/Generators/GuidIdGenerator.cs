using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Generators
{
    public class GuidIdGenerator : IIdGenerator
    {
        public string NewId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
