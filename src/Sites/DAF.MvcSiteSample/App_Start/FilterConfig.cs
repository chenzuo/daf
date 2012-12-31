using System.Web;
using System.Web.Mvc;

namespace DAF.MvcSiteSample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DAF.Web.Mvc.AntiForgeryAuthorizationFilter());
        }
    }
}