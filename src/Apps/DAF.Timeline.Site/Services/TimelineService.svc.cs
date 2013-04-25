using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Timeline;
using DAF.Timeline.Models;
using DAF.Timeline.Info;
using DAF.Timeline.WcfProviders;

namespace DAF.Timeline.Site.Services
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“TimelineService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 TimelineService.svc 或 TimelineService.svc.cs，然后开始调试。
    public class TimelineService : IWcfTimelineService
    {
        private ITimelineProvider provider;

        public TimelineService()
        {
            this.provider = IocInstance.Container.Resolve<ITimelineProvider>();
        }

        public IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null)
        {
            return provider.LoadItems(client, userId, beginTime, count, eventTypes);
        }

        public bool SaveActivity(TimelineItemInfo obj)
        {
            var item = new TimelineItem()
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
            return provider.SaveActivity(item);
        }

        public void MoveItemsToHistory(DateTime endTime)
        {
            provider.MoveItemsToHistory(endTime);
        }
    }
}
