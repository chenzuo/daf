using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Timeline.Messages
{
    [Serializable]
    public class TimelineMessage
    {
        public string ClientId { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }

        public string Title { get; set; }
        public string Decription { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; }
        public string LinkUrl { get; set; }
        public string UserUrl { get; set; }
        public string SiteName { get; set; }
        public string SiteUrl { get; set; }
        public DateTime ActionTime { get; set; }

        public string Keywords { get; set; }
    }
}
