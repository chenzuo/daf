using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.CMS
{
    public class WidgetType
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Module { get; set; }
        public string Category { get; set; }
        public IEnumerable<WidgetParameter> Parameters { get; set; }
    }

    public class WidgetParameter
    {
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
    }
}
