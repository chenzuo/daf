using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Web
{
    public class ContentItem
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public int VisitCount { get; set; }
        public DateTime PublishTime { get; set; }
        public int Status { get; set; }
        public int ShowOrder { get; set; }

        public string CssClass { get; set; }
        public string Target { get; set; }

        public IEnumerable<ContentItem> Items { get; set; }
    }
}
