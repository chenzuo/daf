using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public class TemplateType
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public IEnumerable<PageSection> Sections { get; set; }
    }
}
