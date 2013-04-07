using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public class TemplateType
    {
        public TemplateType()
        {
            Children = new List<TemplateType>();
            Sections = new List<PageSection>();
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentName { get; set; }
        public ICollection<PageSection> Sections { get; set; }
        public ICollection<TemplateType> Children { get; set; }
    }
}
