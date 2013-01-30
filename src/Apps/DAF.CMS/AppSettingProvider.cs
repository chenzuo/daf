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
    public class AppSettingProvider : IAppSettingProvider
    {
        private ITransactionManager trans;
        private IRepository<AppSetting> repoAppSetting;

        public AppSettingProvider(ITransactionManager trans, IRepository<AppSetting> repoAppSetting)
        {
            this.trans = trans;
            this.repoAppSetting = repoAppSetting;
        }

        public IEnumerable<AppSetting> Query(string siteId, string category = null, string name = null)
        {
            var query = repoAppSetting.Query(o => o.SiteId == siteId);
            if (!string.IsNullOrEmpty(category))
                query = query.Where(o => o.Category == category);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(o => o.Name == name);
            query = query.OrderBy(o => o.Category).ThenBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public IEnumerable<string> GetCategoryNames(string siteId)
        {
            return repoAppSetting.Query(a => a.SiteId == siteId).Select(a => a.Category).Distinct().ToArray();
        }

        public bool Save(ChangedData<AppSetting> items)
        {
            return repoAppSetting.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }
    }
}
