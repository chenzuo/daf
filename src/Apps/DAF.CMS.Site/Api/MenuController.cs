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
    public class MenuController : ApiController
    {
        private IMenuProvider provider;

        public MenuController(IMenuProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<SiteMenuGroup> Groups(string siteId)
        {
            return provider.GetGroups(siteId);
        }

        [HttpGet]
        public IEnumerable<SiteMenuItem> Data(string siteId, string group = null, string parentName = null)
        {
            return provider.GetMenu(siteId, group, parentName);
        }

        //[HttpGet]
        //public IEnumerable<SiteMenuItem> Data(string siteId, string group)
        //{
        //    var items = provider.GetMenu(siteId, group, null);
        //    HierarchyHelper.DoDescendants(items, m => m.Children, m => m.Children == null || m.Children.Count() <= 0,
        //                                m => m.Children = provider.GetMenu(siteId, group, m.Name).ToList());

        //    return items;
        //}

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<SiteMenuItem> items)
        {
            return items.Save(o => provider.Save(items));
        }

        [HttpPost]
        public ServerResponse AddGroup(SiteMenuGroup group)
        {
            return group.Save(o => provider.AddGroup(o));
        }

        [HttpPost]
        public ServerResponse EditGroup(SiteMenuGroup group)
        {
            return group.Save(o => provider.UpdateGroup(o));
        }

        [HttpGet]
        public ServerResponse DeleteGroup(string siteId, string group)
        {
            return siteId.Save(o => provider.DeleteGroup(siteId, group),
                LocaleHelper.Localizer.Get("DeleteSuccessfully"), LocaleHelper.Localizer.Get("DeleteFailure"));
        }
    }
}