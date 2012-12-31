using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Logging
{
    public interface ILogger
    {
        bool IsEnabled(LogLevel level);
        void Log(LogLevel level, Exception exception, string format, params object[] args);
    }
}
