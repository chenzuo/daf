using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Core.Serialization;

namespace DAF.CMS.Models
{
    [Table("cms_Content")]
    public class Content : DAF.Core.IOrdered
    {
        public Content()
        {
            Categories = new List<CategoryContent>();
            RelatedContents = new List<ContentRelation>();
        }

        [Required]
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string SiteId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string ContentId { get; set; }
        public ContentType ContentType { get; set; }

        [StringLength(100)]
        public virtual string Title { get; set; }
        [StringLength(200)]
        public virtual string Keywords { get; set; }
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Url)]
        [StringLength(200)]
        public string ImageUrl { get; set; }
        [DataType(DataType.Url)]
        [StringLength(200)]
        public string ContentUrl { get; set; }
        [DataType(DataType.Url)]
        [StringLength(200)]
        public string LinkUrl { get; set; }
        [DataType(DataType.Url)]
        [StringLength(200)]
        public string ShortUrl { get; set; }
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public string PlainBody { get; set; }
        [MaxLength]
        [DataType(DataType.Html)]
        public string HtmlBody { get; set; }
        [MaxLength]
        public string Properties { get; set; }

        public double? ContentSize { get; set; }

        public bool Published { get; set; }
        public int ReadCount { get; set; }
        public bool CreateAsRelated { get; set; }

        [StringLength(50)]
        public string CreatorId { get; set; }
        [StringLength(50)]
        public string CreatorName { get; set; }
        public DateTime? CreateTime { get; set; }

        [StringLength(50)]
        public string ModifierId { get; set; }
        [StringLength(50)]
        public string ModifierName { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [StringLength(50)]
        public string PublisherId { get; set; }
        [StringLength(50)]
        public string PublisherName { get; set; }
        public DateTime? PublishTime { get; set; }

        public int ShowOrder { get; set; }

        [ForeignKey("SiteId")]
        public virtual SubSite Site { get; set; }
        public virtual ICollection<CategoryContent> Categories { get; set; }
        public virtual ICollection<ContentRelation> RelatedContents { get; set; }
    }

    public static class ContentExtensions
    {
        public static T Property<T>(this Content obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.Properties))
                return default(T);

            IJsonSerializer json = IOC.Current.GetService<IJsonSerializer>();
            return json.Deserialize<T>(obj.Properties);
        }

        public static void SetProperty(this Content obj, object props)
        {
            if (obj == null)
                return;
            IJsonSerializer json = IOC.Current.GetService<IJsonSerializer>();
            obj.Properties = json.Serialize(props);
        }
    }
}