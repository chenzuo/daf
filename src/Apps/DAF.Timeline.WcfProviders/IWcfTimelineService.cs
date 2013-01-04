using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DAF.Core;
using DAF.Timeline.Models;
using DAF.Timeline.Info;

namespace DAF.Timeline.WcfProviders
{
    [ServiceContract]
    public interface IWcfTimelineService
    {
        [OperationContract]
        IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null);
        [OperationContract]
        bool SaveActivity(TimelineItemInfo obj);
        [OperationContract]
        void MoveItemsToHistory(DateTime endTime);
    }
}
