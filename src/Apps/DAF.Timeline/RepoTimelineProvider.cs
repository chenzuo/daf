using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Timeline.Models;

namespace DAF.Timeline
{
    public class RepoTimelineProvider : ITimelineProvider
    {
        private IIdGenerator generator;
        private ITransactionManager trans;
        private IRepository<TimelineItem> repoTi;
        private IRepository<TimelineItemHistory> repoTih;

        public RepoTimelineProvider(IIdGenerator generator, ITransactionManager trans, IRepository<TimelineItem> repoTi, IRepository<TimelineItemHistory> repoTih)
        {
            this.generator = generator;
            this.trans = trans;
            this.repoTi = repoTi;
            this.repoTih = repoTih;
        }
        
        public IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null)
        {
            string[] ets = null;
            if (!string.IsNullOrEmpty(eventTypes))
            {
                ets = eventTypes.Split(new char[','], StringSplitOptions.RemoveEmptyEntries);
            }

            var items = repoTi.Query(o => o.ClientId == client && o.UserId == userId && o.ActionTime >= beginTime);
            if (ets != null)
            {
                items = items.Where(o => ets.Contains(o.EventType));
            }
            var minTime = repoTi.Query(null).Min(o => o.ActionTime);
            if (count > 0)
            {
                var total = items.Count();
                if (total < count && minTime > beginTime)
                {
                    var oitems = repoTih.Query(o => o.ClientId == client && o.UserId == userId && o.ActionTime >= beginTime);
                    if (ets != null)
                    {
                        oitems = oitems.Where(o => ets.Contains(o.EventType));
                    }
                    items = items.Union(oitems);
                }
                items = items.OrderByDescending(o => o.ActionTime);
                items = items.Take(count);
            }
            else
            {
                if (minTime > beginTime)
                {
                    var oitems = repoTih.Query(o => o.ClientId == client && o.UserId == userId && o.ActionTime >= beginTime);
                    if (ets != null)
                    {
                        oitems = oitems.Where(o => ets.Contains(o.EventType));
                    }
                    items = items.Union(oitems);
                }
                items = items.OrderByDescending(o => o.ActionTime);
            }
            return items.ToArray();
        }

        public bool SaveActivity(TimelineItem obj)
        {
            if (string.IsNullOrEmpty(obj.ItemId))
            {
                obj.ItemId = generator.NewId();
            }
            return repoTi.Save(o => o.ItemId == obj.ItemId, obj);
        }

        public void MoveItemsToHistory(DateTime endTime)
        {
            endTime = endTime.Date;
            try
            {
                trans.BeginTransaction();
                var items = repoTi.Query(o => o.ActionTime < endTime);
                foreach (var item in items)
                {
                    var obj = new TimelineItemHistory()
                    {
                        ItemId = item.ItemId,
                        ClientId = item.ClientId,
                        EventType = item.EventType,
                        EventName = item.EventName,
                        UserId = item.UserId,
                        UserName = item.UserName,
                        UserType = item.UserType,
                        Title = item.Title,
                        Decription = item.Decription,
                        ImageUrl = item.ImageUrl,
                        DetailUrl = item.DetailUrl,
                        LinkUrl = item.LinkUrl,
                        UserUrl = item.UserUrl,
                        SiteName = item.SiteName,
                        SiteUrl = item.SiteUrl,
                        ActionTime = item.ActionTime,
                        Keywords = item.Keywords
                    };
                    repoTih.Insert(obj);
                    repoTi.Delete(item);
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
        }
    }
}
