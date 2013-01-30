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
    public class BasicDataProvider : IBasicDataProvider
    {
        private ITransactionManager trans;
        private IRepository<BasicDataItem> repoBasicDataItem;

        public BasicDataProvider(ITransactionManager trans, IRepository<BasicDataItem> repoBasicDataItem)
        {
            this.trans = trans;
            this.repoBasicDataItem = repoBasicDataItem;
        }

        public IEnumerable<BasicDataItem> Query(string siteId, string category, string groupName, string name, string parentId = null, bool? isValid = true)
        {
            var query = repoBasicDataItem.Query(o => o.SiteId == siteId);
            if (!string.IsNullOrEmpty(category))
                query = query.Where(o => o.Category == category);
            if (!string.IsNullOrEmpty(groupName))
                query = query.Where(o => o.GroupName == groupName);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(o => o.Name == name);
            if (string.IsNullOrEmpty(parentId))
                query = query.Where(o => o.ParentId == null);
            else
                query = query.Where(o => o.ParentId == parentId);
            if (isValid.HasValue)
                query = query.Where(o => o.IsValid == isValid.Value);

            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }


        public IEnumerable<string> GetCategoryNames(string siteId)
        {
            return repoBasicDataItem.Query(a => a.SiteId == siteId).Select(a => a.Category).Distinct().ToArray();
        }

        public bool Save(ChangedData<BasicDataItem> items)
        {
            return repoBasicDataItem.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
