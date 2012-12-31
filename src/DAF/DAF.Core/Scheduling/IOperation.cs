using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Scheduling
{
    public interface IOperation
    {
        void Start(Dictionary<string, string> paras);
        void Active(Dictionary<string, string> paras);
        void HandleError(Exception ex);
        void Stop(Dictionary<string, string> paras);

        string Name { get; }
        string Description { get; }
    }
}
