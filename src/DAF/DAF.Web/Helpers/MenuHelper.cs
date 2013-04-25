using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Web.Menu;

namespace DAF.Web
{
    public class MenuHelper
    {
        public static IEnumerable<MenuItem> GetMenuItems(string group)
        {
            var menuGroupProvider = IocInstance.Container.Resolve<IObjectProvider<IEnumerable<MenuGroup>>>();
            if (menuGroupProvider != null)
            {
                var mgs = menuGroupProvider.GetObject();
                var mg = mgs.FirstOrDefault(o => o.Name == group);
                if (mg != null)
                {
                    return mg.MenuItems;
                }
            }
            return Enumerable.Empty<MenuItem>();
        }
    }
}
