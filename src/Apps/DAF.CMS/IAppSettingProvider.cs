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
    public interface IAppSettingProvider
    {
        IEnumerable<AppSetting> Query(string siteId, string category = null, string name = null);
        IEnumerable<string> GetCategoryNames(string siteId);
        bool Save(ChangedData<AppSetting> items);
    }
}
