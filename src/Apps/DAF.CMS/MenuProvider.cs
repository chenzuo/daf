using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Collections;
using DAF.Web;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class MenuProvider : IMenuProvider
    {
        private ITransactionManager trans;
        private IRepository<SiteMenuGroup> repoMenuGroup;
        private IRepository<SiteMenuItem> repoMenuItem;

        public MenuProvider(ITransactionManager trans, IRepository<SiteMenuGroup> repoMenuGroup, IRepository<SiteMenuItem> repoMenuItem)
        {
            this.trans = trans;
            this.repoMenuGroup = repoMenuGroup;
            this.repoMenuItem = repoMenuItem;
        }

        public IEnumerable<SiteMenuItem> GetMenu(string siteId, string groupName)
        {
            var query = repoMenuItem.Query(o => o.SiteId == siteId && o.MenuGroupName == groupName);
            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public IEnumerable<SiteMenuItem> GetMenu(string siteId, string groupName, string parentName)
        {
            var query = repoMenuItem.Query(o => o.SiteId == siteId && o.MenuGroupName == groupName);
            if (!string.IsNullOrEmpty(parentName))
                query = query.Where(o => o.ParentName == parentName);
            else
                query = query.Where(o => o.ParentName == null);
            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public ICollection<TreeNode> GetMenuTree(string siteId, string groupName)
        {
            var query = repoMenuItem.Query(o => o.SiteId == siteId && o.MenuGroupName == groupName).ToArray();
            var tree = HierarchyHelper.Build<SiteMenuItem>(query.Where(o => o.ParentName == null),
                o => o.Name, o => o.Caption, o => query.Where(s => s.ParentName == o.Name));

            return tree;
        }

        public IEnumerable<SiteMenuGroup> GetGroups(string siteId)
        {
            return repoMenuGroup.Query(o => o.SiteId == siteId).OrderBy(o => o.Name).ToArray();
        }

        public bool Save(ChangedData<SiteMenuItem> items)
        {
            return repoMenuItem.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }


        public bool AddGroup(SiteMenuGroup group)
        {
            return repoMenuGroup.Insert(group);
        }

        public bool UpdateGroup(SiteMenuGroup group)
        {
            return repoMenuGroup.Update(group);
        }

        public bool DeleteGroup(string siteId, string group)
        {
            return repoMenuGroup.DeleteBatch(o => o.SiteId == siteId && o.Name == group);
        }
    }
}
