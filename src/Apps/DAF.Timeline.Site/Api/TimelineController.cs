using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Web;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Timeline.Site.Models;

namespace DAF.Timeline.Site.Api
{
    public class TimelineController : ApiController
    {
        private IIdGenerator generator;
        private IRepository<TimelineItem> repoTi;

        public TimelineController(IIdGenerator generator, IRepository<TimelineItem> repoTi)
        {
            this.generator = generator;
            this.repoTi = repoTi;
        }

        [HttpGet]
        public IEnumerable<TimelineItem> Items(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null)
        {
            var items = repoTi.Query(o => o.ClientId == client && o.UserId == userId && o.ActionTime >= beginTime);
            if (!string.IsNullOrEmpty(eventTypes))
            {
                string[] ets = eventTypes.Split(new char[','], StringSplitOptions.RemoveEmptyEntries);
                items = items.Where(o => ets.Contains(o.EventType));
            }
            if (count > 0)
            {
                items = items.Take(count);
            }
            return items.ToArray();
        }

        [HttpGet]
        public ServerResponse AddActivity(string client, string userId, string eventType, string eventName, DateTime actionTime,
            string userType, string userName, string title, string desp, 
            string imgUrl, string detailUrl, string linkUrl, string userUrl, string siteName, string siteUrl,
            string keywords)
        {
            var obj = new TimelineItem()
            {
                ItemId = generator.NewId(),
                ClientId = client,
                UserId = userId,
                EventType = eventType,
                EventName = eventName,
                UserType = userType,
                UserName = userName,
                Title = title,
                Decription = desp,
                ImageUrl = imgUrl,
                DetailUrl = detailUrl,
                LinkUrl = linkUrl,
                UserUrl = userUrl,
                SiteName = siteName,
                SiteUrl = siteUrl,
                ActionTime = actionTime,
                Keywords = keywords
            };

            ServerResponse result = new ServerResponse();
            try
            {
                if (repoTi.Insert(obj))
                {
                    result.Status = ResponseStatus.Success;
                }
                else
                {
                    result.Status = ResponseStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        public ServerResponse AddActivity([FromBody]TimelineItem obj)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                if (repoTi.Insert(obj))
                {
                    result.Status = ResponseStatus.Success;
                }
                else
                {
                    result.Status = ResponseStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}