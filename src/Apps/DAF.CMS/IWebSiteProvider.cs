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
    public interface IWebSiteProvider
    {
        IEnumerable<WebSite> Query();
        bool SaveWebSite(WebSite obj);
        bool RemoveWebSite(string siteName);
        SubSite GetSiteById(string siteId);
        SubSite GetSiteByUrl(string url, string locale = null);
        SubSite GetSiteByName(string siteName, string locale = null);
        IEnumerable<SubSite> GetSubSites(string siteName);
        bool SaveSubSites(ChangedData<SubSite> items);
    }
}
