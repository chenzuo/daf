using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Workflow
{
    [Serializable]
    public class FlowOutcomeNotProvidedException : Exception
    {
        public FlowOutcomeNotProvidedException(string message)
            : base(message)
        {
        }
    }
}
