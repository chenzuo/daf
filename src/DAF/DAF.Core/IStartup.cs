using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core
{
    public interface IStartup
    {
        void OnStarted();
        int ExecuteOrder { get; }
    }
}
