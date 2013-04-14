using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Net.Http;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Helpers;
using System.Web.Routing;
using DAF.Core;
using DAF.Core.Caching;
using DAF.Web.Mvc;

namespace DAF.Web
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString CachePartial<T>(this HtmlHelper<T> html, string key, int cacheSeconds, string partialName, object model = null, ViewDataDictionary viewData = null, bool isSessionCache = false)
        {
            return (html as HtmlHelper).CachePartial(key, cacheSeconds, partialName, model, viewData, isSessionCache);
        }

        public static IHtmlString CachePartial(this HtmlHelper html, string key, int cacheSeconds, string partialName, object model = null, ViewDataDictionary viewData = null, bool isSessionCache = false)
        {
#if !DEBUG
            var cacheManager = WebHelper.GetService<ICacheManager>();
            ICacheProvider cache = null;
            if (isSessionCache)
                cache = cacheManager.CreateCacheProvider(CacheScope.WorkUnit);
            else
                cache = cacheManager.CreateCacheProvider(CacheScope.Global);

            if (cacheSeconds > 0 && !string.IsNullOrWhiteSpace(key) && cache != null)
            {
                IHtmlString content = null;

                content = cache.GetData(key) as IHtmlString;

                if (content != null)
                    return content;

                content = html.Partial(partialName, model, viewData);

                cache.Add(key, content, null, new TimeSpan(0, 0, cacheSeconds), DateTime.MaxValue);

                return content;
            }
#endif
            return html.Partial(partialName, model, viewData);
        }

        public static IHtmlString CacheAction<T>(this HtmlHelper<T> html, string key, int cacheSeconds, string action, string controller = null, object routes = null, bool isSessionCache = false)
        {
            return (html as HtmlHelper).CacheAction(key, cacheSeconds, action, controller, routes, isSessionCache);
        }

        public static IHtmlString CacheAction(this HtmlHelper html, string key, int cacheSeconds, string action, string controller = null, object routes = null, bool isSessionCache = false)
        {
#if !DEBUG
            var cacheManager = WebHelper.GetService<ICacheManager>();
            ICacheProvider cache = null;
            if (isSessionCache)
                cache = cacheManager.CreateCacheProvider(CacheScope.WorkUnit);
            else
                cache = cacheManager.CreateCacheProvider(CacheScope.Global);

            if (cacheSeconds > 0 && !string.IsNullOrWhiteSpace(key) && cache != null)
            {
                IHtmlString content = null;

                content = cache.GetData(key) as IHtmlString;

                if (content != null)
                    return content;

                content = html.Action(action, controller, routes);

                cache.Add(key, content, null, new TimeSpan(0, 0, cacheSeconds), DateTime.MaxValue);

                return content;
            }
#endif
            return html.Action(action, controller, routes);
        }

        public static IHtmlString Widget(this HtmlHelper html, string url)
        {
            var request = html.ViewContext.HttpContext.Request;
            Uri referer = request.UrlReferrer ?? request.Url;
            string content = HttpHelper.Get(url, null, request.UserAgent, referer, LocaleHelper.Localizer.GetCurrentCultureInfo());
            return new HtmlString(content);
        }

        public static IHtmlString CacheWidget(this HtmlHelper html, string key, int cacheSeconds, string url, bool isSessionCache = false)
        {
#if !DEBUG
            var cacheManager = WebHelper.GetService<ICacheManager>();
            ICacheProvider cache = null;
            if (isSessionCache)
                cache = cacheManager.CreateCacheProvider(CacheScope.WorkUnit);
            else
                cache = cacheManager.CreateCacheProvider(CacheScope.Global);

            if (cacheSeconds > 0 && !string.IsNullOrWhiteSpace(key) && cache != null)
            {
                IHtmlString content = null;

                content = cache.GetData(key) as IHtmlString;

                if (content != null)
                    return content;

                content = html.Widget(url);

                cache.Add(key, content, null, new TimeSpan(0, 0, cacheSeconds), DateTime.MaxValue);

                return content;
            }
#endif
            return html.Widget(url);
        }

        public static IHtmlString Widget<T>(this HtmlHelper html, string url, WidgetBag<T> bag)
            where T : class
        {
            var request = html.ViewContext.HttpContext.Request;
            Uri referer = request.UrlReferrer ?? request.Url;
            string content = HttpHelper.Post<WidgetBag<T>>(url, bag, null, request.UserAgent, referer, LocaleHelper.Localizer.GetCurrentCultureInfo());
            return new HtmlString(content);
        }

        public static IHtmlString CacheWidget<T>(this HtmlHelper html, string key, int cacheSeconds, string url, WidgetBag<T> bag, bool isSessionCache = false)
            where T : class
        {
#if !DEBUG
            var cacheManager = WebHelper.GetService<ICacheManager>();
            ICacheProvider cache = null;
            if (isSessionCache)
                cache = cacheManager.CreateCacheProvider(CacheScope.WorkUnit);
            else
                cache = cacheManager.CreateCacheProvider(CacheScope.Global);

            if (cacheSeconds > 0 && !string.IsNullOrWhiteSpace(key) && cache != null)
            {
                IHtmlString content = null;

                content = cache.GetData(key) as IHtmlString;

                if (content != null)
                    return content;

                content = html.Widget<T>(url, bag);

                cache.Add(key, content, null, new TimeSpan(0, 0, cacheSeconds), DateTime.MaxValue);

                return content;
            }
#endif
            return html.Widget<T>(url, bag);
        }

        public static IHtmlString AppendQueryString(this HtmlHelper html, string querystring)
        {
            string rawUrl = html.ViewContext.HttpContext.Request.RawUrl;
            if (string.IsNullOrWhiteSpace(querystring))
                return new HtmlString(rawUrl);
            var uqs = querystring.ToDictionary();

            foreach (var q in html.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (uqs.ContainsKey(q))
                    continue;
                uqs.Add(q, html.ViewContext.HttpContext.Request.QueryString[q]);
            }

            rawUrl = string.Concat(html.ViewContext.HttpContext.Request.Path, "?", uqs.ToFormatString());
            return new HtmlString(rawUrl);
        }
    }
}
