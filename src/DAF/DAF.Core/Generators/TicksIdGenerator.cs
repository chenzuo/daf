using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Generators
{
    public class TicksIdGenerator : IIdGenerator
    {
        public string NewId()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}
