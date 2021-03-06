﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Collections;
using DAF.Web;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public interface IMenuProvider
    {
        IEnumerable<SiteMenuGroup> GetGroups(string siteId);
        IEnumerable<SiteMenuItem> GetMenu(string siteId, string groupName);
        IEnumerable<SiteMenuItem> GetMenu(string siteId, string groupName, string parentName);

        bool Save(ChangedData<SiteMenuItem> items);

        bool AddGroup(SiteMenuGroup group);
        bool UpdateGroup(SiteMenuGroup group);
        bool DeleteGroup(string siteId, string group);
    }
}
