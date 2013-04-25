using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Scheduling
{
    public interface IOperation
    {
        void Start(IDictionary<string, string> paras);
        void Active(IDictionary<string, string> paras);
        void HandleError(Exception ex);
        void Stop(IDictionary<string, string> paras);

        string Name { get; }
        string Description { get; }
    }
}
