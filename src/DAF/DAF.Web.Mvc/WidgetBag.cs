using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace DAF.Web.Mvc
{
    public class WidgetBag<T>
    {
        public static WidgetBag<TB> Create<TB>(string widgetId = null, string cssClass = null, TB model = null, object args = null)
            where TB : class
        {
            WidgetBag<TB> bag = new WidgetBag<TB>();
            bag.WidgetId = widgetId;
            bag.CssClass = cssClass;
            bag.Model = model;
            if (args != null)
                bag.Arguments = new RouteValueDictionary(args);
            else
                bag.Arguments = new RouteValueDictionary();

            return bag;
        }

        public string WidgetId { get; set; }
        public string CssClass { get; set; }
        public T Model { get; set; }
        public RouteValueDictionary Arguments { get; set; }
    }
}
