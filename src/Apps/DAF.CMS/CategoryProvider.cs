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
    public class CategoryProvider : ICategoryProvider
    {
        private ITransactionManager trans;
        private IRepository<Category> repoCategory;

        public CategoryProvider(ITransactionManager trans, IRepository<Category> repoCategory)
        {
            this.trans = trans;
            this.repoCategory = repoCategory;
        }

        public IEnumerable<Category> Query(string siteId, string groupName, string code = null, string parentId = null, DataStatus? status = DataStatus.Normal)
        {
            var query = repoCategory.Query(o => o.SiteId == siteId && o.Status == status);
            if (!string.IsNullOrEmpty(groupName))
                query = query.Where(o => o.GroupName == groupName);
            if (!string.IsNullOrEmpty(code))
                query = query.Where(o => o.Code == code);
            if (!string.IsNullOrEmpty(parentId))
                query = query.Where(o => o.ParentId == parentId);
            else
                query = query.Where(o => o.ParentId == null);
            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public bool Save(ChangedData<Category> items)
        {
            return repoCategory.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
