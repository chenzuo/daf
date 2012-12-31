using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Logging;

namespace DAF.Core
{
    public abstract class ProviderBaseWithLogger
    {
        public ProviderBaseWithLogger()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
    }
}
