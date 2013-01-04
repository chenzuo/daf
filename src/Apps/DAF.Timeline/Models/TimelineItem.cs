using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Timeline.Models
{
    [Table("tl_TimelineItem")]
    public class TimelineItem
    {
        [Required]
        [Key]
        [StringLength(50)]
        public string ItemId { get; set; }
        [Required]
        [StringLength(50)]
        public string ClientId { get; set; }
        [Required]
        [StringLength(50)]
        public string EventType { get; set; }
        [StringLength(50)]
        public string EventName { get; set; }
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(10)]
        public string UserType { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Decription { get; set; }
        [StringLength(200)]
        public string ImageUrl { get; set; }
        [StringLength(200)]
        public string DetailUrl { get; set; }
        [StringLength(200)]
        public string LinkUrl { get; set; }
        [StringLength(200)]
        public string UserUrl { get; set; }
        [StringLength(50)]
        public string SiteName { get; set; }
        private string siteUrl;
        [StringLength(200)]
        public string SiteUrl
        {
            get
            {
                return siteUrl;
            }
            set
            {
                siteUrl = value.ToLower();
            }
        }
        public DateTime ActionTime { get; set; }

        public string Keywords { get; set; }
    }
}