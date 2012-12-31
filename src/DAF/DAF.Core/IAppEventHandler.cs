using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core
{
    public interface IAppEventHandler
    {
        void OnApplicationStart(IContainer container, object context);
        void OnApplicatoinExit(IContainer container, object context);
        int ExecuteOrder { get; }
    }
}
