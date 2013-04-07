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
        public IEnumerable<Category> Data(string siteId, string parentId = null)
        {
            return cateProvider.GetSubCategories(siteId, parentId);
        }

        [HttpGet]
        public IEnumerable<Category> Tree(string siteId)
        {
            var allEles = cateProvider.Query(siteId);
            var cates = HierarchyHelper.Build(allEles, o => o.ParentId == null, (p, c) => c.ParentId == p.CategoryId, (p, c) => p.Children.Add(c));
            return cates;
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<Category> items)
        {
            return items.Save(o => cateProvider.Save(o));
        }
    }
}