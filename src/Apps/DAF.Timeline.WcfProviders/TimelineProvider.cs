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
    public class TimelineProvider : ITimelineProvider
    {
        private ChannelFactory<IWcfTimelineService> CreateChannel()
        {
            return WcfService.CreateChannel<IWcfTimelineService>("IWcfTimelineService");
        }

        public IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null)
        {
            Assert.IsStringNotNullOrEmpty(client);
            Assert.IsStringNotNullOrEmpty(userId);

            IEnumerable<TimelineItem> objs = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                objs = p.LoadItems(client, userId, beginTime, count, eventTypes);
            });

            return objs;
        }

        public bool SaveActivity(TimelineItem obj)
        {
            Assert.IsNotNull(obj);

            TimelineItemInfo info = new TimelineItemInfo()
            {
                ClientId = obj.ClientId,
                EventType = obj.EventType,
                EventName = obj.EventName,
                UserId = obj.UserId,
                UserName = obj.UserName,
                UserType = obj.UserType,
                Title = obj.Title,
                Decription = obj.Decription,
                ImageUrl = obj.ImageUrl,
                DetailUrl = obj.DetailUrl,
                LinkUrl = obj.LinkUrl,
                UserUrl = obj.UserUrl,
                SiteName = obj.SiteName,
                SiteUrl = obj.SiteUrl,
                ActionTime = obj.ActionTime,
                Keywords = obj.Keywords
            };

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.SaveActivity(info);
            });

            return result;
        }

        public void MoveItemsToHistory(DateTime endTime)
        {
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                p.MoveItemsToHistory(endTime);
            });
        }
    }
}
