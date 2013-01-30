using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Web;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class WebSiteProvider : IWebSiteProvider
    {
        private ITransactionManager trans;
        private IRepository<WebSite> repoWebSite;
        private IRepository<SubSite> repoLocSite;

        public WebSiteProvider(ITransactionManager trans, IRepository<WebSite> repoWebSite, IRepository<SubSite> repoLocSite)
        {
            this.trans = trans;
            this.repoWebSite = repoWebSite;
            this.repoLocSite = repoLocSite;
        }

        public IEnumerable<WebSite> Query()
        {
            return repoWebSite.Query(null).ToArray();
        }

        public bool SaveWebSite(WebSite obj)
        {
            return repoWebSite.Save(o => o.SiteName == obj.SiteName, obj);
        }

        public bool RemoveWebSite(string siteName)
        {
            return repoWebSite.DeleteBatch(o => o.SiteName == siteName);
        }

        public SubSite GetSiteById(string siteId)
        {
            return repoLocSite.Query(o => o.SiteId == siteId).FirstOrDefault();
        }

        public SubSite GetSiteByUrl(string url, string locale = null)
        {
            if (locale == null)
                locale = LocaleHelper.Localizer.GetCurrentCultureInfo();
            url = url.ToLower();
            locale = locale.ToLower();
            var site = repoLocSite.Query(o => url.StartsWith(o.OwnerSite.UrlStartWith) && o.SubSiteName == locale).FirstOrDefault();
            return site;
        }

        public SubSite GetSiteByName(string siteName, string locale = null)
        {
            if (locale == null)
                locale = LocaleHelper.Localizer.GetCurrentCultureInfo().ToLower();
            var site = repoLocSite.Query(o => o.OwnerSite.SiteName == siteName && o.SubSiteName == locale).FirstOrDefault();
            return site;
        }

        public IEnumerable<SubSite> GetSubSites(string siteName)
        {
            return repoLocSite.Query(o => o.SiteName == siteName).ToArray();
        }

        public bool SaveSubSites(ChangedData<SubSite> items)
        {
            return repoLocSite.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
