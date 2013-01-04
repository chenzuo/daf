using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using DAF.Core;
using DAF.Timeline;
using DAF.Timeline.Messages;
using DAF.Timeline.Models;

namespace DAF.Timeline.MessageHandlers
{
    public class TimelineMessageHandler : IHandleMessages<TimelineMessage>
    {
        private ITimelineProvider provider;

        public TimelineMessageHandler(ITimelineProvider provider)
        {
            this.provider = provider;
        }

        public void Handle(TimelineMessage message)
        {
            TimelineItem obj = new TimelineItem()
            {
                ClientId = message.ClientId,
                EventType = message.EventType,
                EventName = message.EventName,
                UserId = message.UserId,
                UserName = message.UserName,
                UserType = message.UserType,
                Title = message.Title,
                Decription = message.Decription,
                ImageUrl = message.ImageUrl,
                DetailUrl = message.DetailUrl,
                LinkUrl = message.LinkUrl,
                UserUrl = message.UserUrl,
                SiteName = message.SiteName,
                SiteUrl = message.SiteUrl,
                ActionTime = message.ActionTime,
                Keywords = message.Keywords
            };

            try
            {
                provider.SaveActivity(obj);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Save Timeline Item Failure");
            }
        }
    }
}
