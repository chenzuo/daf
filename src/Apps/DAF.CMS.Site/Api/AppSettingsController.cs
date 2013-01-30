using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Generators;
using DAF.Web;
using DAF.Web.Menu;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class AppSettingsController : ApiController
    {
        private IAppSettingProvider provider;

        public AppSettingsController(IAppSettingProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<string> Categories(string siteId)
        {
            return provider.GetCategoryNames(siteId);
        }

        [HttpGet]
        public IEnumerable<AppSetting> Data(string siteId, string cate = null)
        {
            return provider.Query(siteId, cate);
        }

        [HttpPost]
        public ServerResponse Save(ChangedData<AppSetting> items)
        {
            return items.Save(o => provider.Save(o));
        }
    }
}