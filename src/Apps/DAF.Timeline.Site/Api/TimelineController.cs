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
using DAF.Timeline;
using DAF.Timeline.Models;

namespace DAF.Timeline.Site.Api
{
    public class TimelineController : ApiController
    {
        private ITimelineProvider provider;

        public TimelineController(ITimelineProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<TimelineItem> LoadItems(string client, string userId, DateTime beginTime, int count = 20, string eventTypes = null)
        {
            return provider.LoadItems(client, userId, beginTime, count, eventTypes);
        }

        [HttpGet]
        public ServerResponse SaveActivity(string client, string userId, string eventType, string eventName, DateTime actionTime,
            string userType, string userName, string title, string desp, 
            string imgUrl, string detailUrl, string linkUrl, string userUrl, string siteName, string siteUrl,
            string keywords)
        {
            var obj = new TimelineItem()
            {
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
                if (provider.SaveActivity(obj))
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
        public ServerResponse SaveActivity([FromBody]TimelineItem obj)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                if (provider.SaveActivity(obj))
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