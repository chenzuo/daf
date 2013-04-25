using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.WebPages;
using System.Web.WebPages.Html;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Caching;

namespace DAF.CMS
{
    public static class WebPageExtensions
    {
        public static IHtmlString RenderCachedPage(this WebPage webpage, string cacheKey, int cacheMunites, string path, params object[] data)
        {
            if (!string.IsNullOrEmpty(cacheKey) && cacheMunites > 0)
            {
                var cm = IocInstance.Container.Resolve<ICacheManager>();
                var cp = cm.CreateCacheProvider(CacheScope.Global);
                var html = cp.GetData(cacheKey) as IHtmlString;
                if (html == null)
                {
                    var helper = webpage.RenderPage(path, data);
                    html = new HtmlString(helper.ToHtmlString());
                    cp.Add(cacheKey, html, null, TimeSpan.FromMinutes(cacheMunites), DateTime.MaxValue);
                }
                return html;
            }
            else
            {
                return webpage.RenderPage(path, data);
            }
        }
    }
}
