using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Collections;
using DAF.Web;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class WebSiteController : ApiController
    {
        private IWebSiteProvider siteProvider;

        public WebSiteController(IWebSiteProvider siteProvider)
        {
            this.siteProvider = siteProvider;
        }

        [HttpGet]
        public IEnumerable<WebSite> Data()
        {
            var sites = siteProvider.Query();
            return sites;
        }

        [HttpGet]
        public ServerResponse Delete(string site)
        {
            return site.Save(o => siteProvider.RemoveWebSite(site));
        }

        [HttpPost]
        public ServerResponse Save([FromBody]WebSite obj)
        {
            return obj.Save(o => siteProvider.SaveWebSite(o));
        }

        [HttpGet]
        public IEnumerable<SubSite> GetSubSites(string site)
        {
            var sites = siteProvider.GetSubSites(site);
            return sites;
        }

        [HttpPost]
        public ServerResponse SaveSubSites([FromBody]ChangedData<SubSite> items)
        {
            return items.Save(o => siteProvider.SaveSubSites(o));
        }
    }
}