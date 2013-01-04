using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Timeline.Models;

namespace DAF.Timeline
{
    public interface ITimelineProvider
    {
        IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null);
        bool SaveActivity(TimelineItem obj);
        void MoveItemsToHistory(DateTime endTime);
    }
}
