using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core;

namespace DAF.Core.Scheduling
{
    public class TimingTrigger : IScheduleTrigger
    {
        public string Name
        {
            get { return "TimingTrigger"; }
        }

        public bool IsTriggered(DateTime signalTime, DateTime lastActiveTime, string lastTriggerValue, Dictionary<string, string> paras, out string currTriggerValue)
        {
            DateTimePart? intervalType = null;
            if(paras.ContainsKey("IntervalType"))
                intervalType = (DateTimePart)Convert.ToInt32(paras["IntervalType"]);
            int? intervalValue = null;
            if (paras.ContainsKey("IntervalValue"))
                intervalValue = Convert.ToInt32(paras["IntervalValue"]);
            DateTime etime = lastActiveTime.Interval(intervalType, intervalValue);
            if (etime <= signalTime)
            {
                currTriggerValue = etime.ToString("yyyy-MM-dd HH:mm:ss");
                return true;
            }
            currTriggerValue = null;
            return false;
        }
    }
}
