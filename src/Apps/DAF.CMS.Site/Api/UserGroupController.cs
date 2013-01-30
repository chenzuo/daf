using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Generators;
using DAF.Web;
using DAF.Web.Menu;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class UserGroupController : ApiController
    {
        private IUserGroupProvider provider;

        public UserGroupController(IUserGroupProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<UserGroup> Data(string siteId, string parentId = null)
        {
            return provider.Query(siteId, parentId);
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<UserGroup> items)
        {
            return items.Save(o => provider.Save(o));
        }

        [HttpGet]
        public IEnumerable<UserGroupUser> Users(string group)
        {
            return provider.GetGroupUsers(group);
        }

        [HttpPost]
        public ServerResponse SaveUsers(string group, [FromBody]UserGroupUser[] objs)
        {
            return objs.Save(o => provider.SaveGroupUsers(group, o));
        }
    }
}