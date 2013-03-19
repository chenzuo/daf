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

        public IEnumerable<Category> GetSubCategories(string siteId, string parentCode, int depth = 1)
        {
            var allCates = repoCategory.Query(o => o.SiteId == siteId).ToArray();
            IQueryable<Category> cates = allCates.AsQueryable();
            if (string.IsNullOrEmpty(parentCode))
                cates = cates.Where(o => o.ParentId == null);
            else
                cates = cates.Where(o => o.Parent.Code == parentCode);
            cates = cates.OrderBy(o => o.ShowOrder);
            if (cates.Count() > 0 && depth > 1)
            {
                Stack<IEnumerable<Category>> st = new Stack<IEnumerable<Category>>();
                st.Push(cates);
                while (st.Count > 0 && depth > 1)
                {
                    var cs = st.Pop();
                    cs.ForEach(o =>
                        {
                            var subCates = allCates.Where(c => c.ParentId == o.CategoryId).OrderBy(c => c.ShowOrder).ToArray();
                            st.Push(subCates);
                            o.Children = subCates;
                        });
                    depth -= 1;
                }
            }

            return cates;
        }

        public bool Save(ChangedData<Category> items)
        {
            return repoCategory.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
