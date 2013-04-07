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
    public interface ICategoryProvider
    {
        IEnumerable<Category> Query(string siteId, string code = null, DataStatus? status = DataStatus.Normal);
        IEnumerable<Category> GetSubCategories(string siteId, string parentId);
        Category GetCategory(string siteId, string idOrCode);
        bool Save(ChangedData<Category> items);
    }
}
