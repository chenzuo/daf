using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Workflow
{
    [Serializable]
    public class FlowIncomeNotProvidedException : Exception
    {
        public FlowIncomeNotProvidedException(string message)
            : base(message)
        {
        }
    }
}
