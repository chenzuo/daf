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
    public interface IUserGroupProvider
    {
        IEnumerable<UserGroup> Query(string siteId, string parentId = null);
        bool Save(ChangedData<UserGroup> items);
        IEnumerable<UserGroupUser> GetGroupUsers(string group);
        bool SaveGroupUsers(string group, UserGroupUser[] objs);
    }
}
