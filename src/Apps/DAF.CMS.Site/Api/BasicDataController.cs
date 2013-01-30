using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Web;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class BasicDataController : ApiController
    {
        private IBasicDataProvider provider;

        public BasicDataController(IBasicDataProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<string> Categories(string siteId)
        {
            return provider.GetCategoryNames(siteId);
        }

        [HttpGet]
        public IEnumerable<BasicDataItem> Data(string siteId, string cate = null, string parentId = null)
        {
            return provider.Query(siteId, cate, null, null, parentId);
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<BasicDataItem> items)
        {
            return items.Save(o => provider.Save(o));
        }
    }
}