using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using DAF.Core.IOC;
using DAF.Core.Logging;

namespace DAF.Core
{
    public class LogHelper
    {
        public static ILogger Logger
        {
            get
            {
                if (IocInstance.Container == null)
                    return NullLogger.Instance;
                var logger = IocInstance.Container.ResolveOptional<ILogger>();
                return logger ?? NullLogger.Instance;
            }
        }
    }
}
