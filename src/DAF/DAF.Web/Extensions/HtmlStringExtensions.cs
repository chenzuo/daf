using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DAF.Web
{
    public static class HtmlStringExtensions
    {
        public static IHtmlString Concact(this IHtmlString html, IHtmlString other)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(html.ToString());
            sb.Append(other.ToString());
            return new HtmlString(sb.ToString());
        }
    }
}
