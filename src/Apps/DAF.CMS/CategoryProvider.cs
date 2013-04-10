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

        public IEnumerable<Category> Query(string siteId, string code = null, DataStatus? status = DataStatus.Normal)
        {
            var query = repoCategory.Query(o => o.SiteId == siteId && o.Status == status);
            if (!string.IsNullOrEmpty(code))
                query = query.Where(o => o.Code == code);
            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public IEnumerable<Category> GetSubCategories(string siteId, string parentId)
        {
            var query = repoCategory.Query(o => o.SiteId == siteId);
            if (string.IsNullOrEmpty(parentId))
                query = query.Where(o => o.ParentId == null);
            else
                query = query.Where(o => (o.ParentId == parentId || o.Parent.Code == parentId));
            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public Category GetCategory(string siteId, string idOrCode)
        {
            return repoCategory.Query(o => o.SiteId == siteId && (o.CategoryId == idOrCode || o.Code == idOrCode)).FirstOrDefault();
        }

        public bool Save(ChangedData<Category> items)
        {
            return repoCategory.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
