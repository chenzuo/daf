using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.WebPages;
using DAF.Core;
using DAF.Web.Api.Metadata.Providers;
using DAF.Web.Api.Metadata;

namespace DAF.Web.Api
{
    public static class WebPageExtensions
    {
        public static PageModel<T> PageModel<T>(this WebPage page)
            where T : class, new()
        {
            return new PageModel<T>(page);
        }

        public static void Refresh(this WebPage page, IServerResponse result = null)
        {
            WebHelper.Refresh(result, page.Response);
        }
    }
}
