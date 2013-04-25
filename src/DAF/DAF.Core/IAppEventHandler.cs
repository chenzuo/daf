using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;

namespace DAF.Core
{
    public interface IAppEventHandler
    {
        void OnApplicationStart(IIocContainer container, object context);
        void OnApplicatoinExit(IIocContainer container, object context);
        int ExecuteOrder { get; }
    }
}
