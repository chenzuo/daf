using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Core.Collections;
using DAF.Web;
using DAF.Web.Menu;
using DAF.CMS.Models;
using DAF.SSO;

namespace DAF.CMS
{
    public class CmsHelper
    {
        public static string GetAppSettingValue(string name)
        {
            var appSetting = GetAppSetting(name);
            return appSetting == null ? string.Format("{0}_NOT_Settting", name) : appSetting.Value;
        }

        public static AppSetting GetAppSetting(string name)
        {
            var appSetting = AppSettingProvider.Query(CurrentSite.SiteId, null, name).FirstOrDefault();
            return appSetting;
        }

        public static IEnumerable<BasicDataItem> GetBasicData(string category, string groupName)
        {
            var items = BasicDataProvider.Query(CurrentSite.SiteId, category, groupName, null, null, true)
                .OrderBy(o => o.ShowOrder);
            return items.ToArray();
        }

        public static IEnumerable<MenuItem> GetMenuItems(string groupName)
        {
            var items = MenuProvider.GetMenu(CurrentSite.SiteId, groupName)
                .Select(o => new MenuItem()
                {
                    Name = o.Name,
                    Caption = o.Caption,
                    LinkUrl = o.LinkUrl,
                    Icon = o.Icon,
                    Target = o.Target,
                    Tooltip = o.Tooltip,
                });
            var menu = HierarchyHelper.Build<MenuItem, MenuItem>(items.Where(o => string.IsNullOrEmpty(o.ParentName)),
                o => o, o => o.Children, (p, c) => p.Children.Add(c));
            return menu;
        }

        public static IEnumerable<Category> GetCategories(string groupName = null, string code = null, string parentId = null, DataStatus? status = DataStatus.Normal, string language = null)
        {
            var query = CategoryProvider.Query(CurrentSite.SiteId, groupName, code, parentId, status);
            return query.ToArray();
        }

        public static IEnumerable<Category> GetSubCategories(string parentCode, int depth = 1)
        {
            var query = CategoryProvider.GetSubCategories(CurrentSite.SiteId, parentCode, depth);
            return query.ToArray();
        }

        public static IEnumerable<Content> GetContents(string category, int pi = 0, int ps = 20)
        {
            var query = ContentProvider.GetContents(CurrentSite.SiteId, category, true, null, null, null, pi, ps);
            return query.ToArray();
        }

        public static Content GetContent(string contentIdOrShortUrl, bool withRelatedContents = true, bool withCategories = true, string language = null)
        {
            if (string.IsNullOrEmpty(contentIdOrShortUrl))
                return null;

            contentIdOrShortUrl = contentIdOrShortUrl.TrimStart('/').Replace("/", "-");
            int idx = contentIdOrShortUrl.IndexOf(".");
            if (idx > 0)
                contentIdOrShortUrl = contentIdOrShortUrl.Substring(0, idx);

            return ContentProvider.Get(CurrentSite.SiteId, contentIdOrShortUrl, withRelatedContents, withCategories);
        }

        public static WebPage GetPage(string pageIdOrNameOrShortUrl)
        {
            var page = PageProvider.GetPageByName(pageIdOrNameOrShortUrl);
            if (page == null)
                page = PageProvider.GetPageByShortUrl(pageIdOrNameOrShortUrl);
            if(page == null)
                page = PageProvider.GetPageById(pageIdOrNameOrShortUrl);
            if (page != null)
            {
                page.Controls = PageProvider.GetControls(page.PageId).ToArray();
                page.Template = PageTemplateProvider.GetTemplate(page.SiteId, page.TemplateName);
                page.Template.Controls = PageTemplateProvider.GetControls(page.SiteId, page.TemplateName).ToArray();
            }
            return page;
        }

        public static Dictionary<string, string> GetControlParas(string paras)
        {
            var dic = paras.ToDictionary<string, string>(o => o, v =>
                {
                    if (v.StartsWith("cmd"))
                    {
                        var cmd = v.Substring(4, v.Length - 5);
                        var val = CommandHelper.Run(cmd, HttpContext.Current);
                        return val == null ? string.Empty : val.ToString();
                    }
                    return v;
                });

            return dic;
        }

        public static SubSite CurrentSite
        {
            get
            {
                SubSite site = null;
                if (HttpContext.Current != null)
                {
                    site = HttpContext.Current.Items["CurrentSite"] as SubSite;
                    if (site != null)
                        return site;
                    if (HttpContext.Current.Session != null)
                    {
                        site = HttpContext.Current.Session["CurrentSite"] as SubSite;
                        if (site != null)
                            return site;
                    }

                    var siteId = HttpContext.Current.Request.QueryString["site"];
                    if (!string.IsNullOrEmpty(siteId))
                    {
                        site = WebSiteProvider.GetSiteById(siteId);
                    }
                    if (site == null)
                    {
                        var url = (HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url).AbsoluteUri;
                        site = WebSiteProvider.GetSiteByUrl(url, LocaleHelper.Localizer.GetCurrentCultureInfo());
                    }
                }
                if (site == null)
                {
                    var siteName = ConfigurationManager.AppSettings["SiteName"];
                    if (!string.IsNullOrEmpty(siteName))
                    {
                        site = WebSiteProvider.GetSiteByName(siteName, LocaleHelper.Localizer.GetCurrentCultureInfo());
                    }
                }
                if (site != null)
                {
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Items.Add("CurrentSite", site);
                        if (HttpContext.Current.Session != null)
                        {
                            HttpContext.Current.Session["CurrentSite"] = site;
                        }
                    }
                }
                return site;
            }
        }

        #region Providers

        public static IWebSiteProvider WebSiteProvider
        {
            get { return IOC.Current.GetService<IWebSiteProvider>(); }
        }

        public static IAppSettingProvider AppSettingProvider
        {
            get { return IOC.Current.GetService<IAppSettingProvider>(); }
        }

        public static IBasicDataProvider BasicDataProvider
        {
            get { return IOC.Current.GetService<IBasicDataProvider>(); }
        }

        public static ICategoryProvider CategoryProvider
        {
            get { return IOC.Current.GetService<ICategoryProvider>(); }
        }

        public static IContentProvider ContentProvider
        {
            get { return IOC.Current.GetService<IContentProvider>(); }
        }

        public static IPageProvider PageProvider
        {
            get { return IOC.Current.GetService<IPageProvider>(); }
        }

        public static IPageTemplateProvider PageTemplateProvider
        {
            get { return IOC.Current.GetService<IPageTemplateProvider>(); }
        }

        public static IMenuProvider MenuProvider
        {
            get { return IOC.Current.GetService<IMenuProvider>(); }
        }

        public static IUserGroupProvider UserGroupProvider
        {
            get { return IOC.Current.GetService<IUserGroupProvider>(); }
        }

        #endregion
    }
}
