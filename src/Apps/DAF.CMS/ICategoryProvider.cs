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
        IEnumerable<Category> Query(string siteId, string groupName, string code = null, string parentId = null, DataStatus? status = DataStatus.Normal);
        IEnumerable<Category> GetSubCategories(string siteId, string parentCode, int depth = 1);
        bool Save(ChangedData<Category> items);
    }
}
