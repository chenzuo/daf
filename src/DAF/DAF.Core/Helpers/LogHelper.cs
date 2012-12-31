using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Autofac;
using DAF.Core.Logging;

namespace DAF.Core
{
    public class LogHelper
    {
        public static ILogger Logger
        {
            get
            {
                if (IOC.Current == null)
                    return NullLogger.Instance;
                var logger = IOC.Current.ResolveOptional<ILogger>();
                return logger ?? NullLogger.Instance;
            }
        }
    }
}
