using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DAF.Core.Scheduling
{
    public interface IScheduleTrigger
    {
        string Name { get; }

        bool IsTriggered(DateTime signalTime, DateTime lastActiveTime, string lastTriggerValue, IDictionary<string, string> paras, out string currTriggerValue);
    }
}
