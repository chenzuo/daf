using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Timeline.Info
{
    [DataContract]
    public class TimelineItemInfo
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string EventType { get; set; }
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserType { get; set; }
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Decription { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string DetailUrl { get; set; }
        [DataMember]
        public string LinkUrl { get; set; }
        [DataMember]
        public string UserUrl { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public string SiteUrl { get; set; }
        [DataMember]
        public DateTime ActionTime { get; set; }
        [DataMember]
        public string Keywords { get; set; }
    }
}
