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
    public class CategoryController : ApiController
    {
        private ICategoryProvider cateProvider;

        public CategoryController(ICategoryProvider cateProvider)
        {
            this.cateProvider = cateProvider;
        }

        [HttpGet]
        public IEnumerable<Category> Data(string siteId, string parentId = null, string code = null)
        {
            return cateProvider.Query(siteId, null, code, parentId);
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<Category> items)
        {
            return items.Save(o => cateProvider.Save(o));
        }

        [HttpGet]
        //[OutputCache(CacheProfile = "common")]
        public IEnumerable<TreeNode> Tree(string siteId, string parentId = null)
        {
            var query = cateProvider.Query(siteId, null, null, parentId);

            var tree = HierarchyHelper.Build(query, o => o.CategoryId, o => o.Name,
                o => cateProvider.Query(siteId, null, null, o.CategoryId));

            return tree;
        }
    }
}