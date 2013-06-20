using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public interface IWidgetTypeProvider
    {
        IEnumerable<WidgetType> LoadWidgetTypes();
        WidgetType GetWidgetType(string nameOrPath);
    }
}
