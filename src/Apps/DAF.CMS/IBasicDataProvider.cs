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
    public interface IBasicDataProvider
    {
        IEnumerable<BasicDataItem> Query(string siteId, string category, string groupName, string name, string parentId = null, bool? isValid = true);
        IEnumerable<string> GetCategoryNames(string siteId);
        bool Save(ChangedData<BasicDataItem> items);
    }
}
