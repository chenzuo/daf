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
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.Controllers
{
    public class UserGroupController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<UserGroup> repoUg;
        private IRepository<UserGroupUser> repoUgu;

        public UserGroupController(ITransactionManager trans, IRepository<UserGroup> repoUg, IRepository<UserGroupUser> repoUgu)
        {
            this.trans = trans;
            this.repoUg = repoUg;
            this.repoUgu = repoUgu;
        }

        [HttpGet]
        public IEnumerable<UserGroup> Data(string parentName = null)
        {
            var query = repoUg.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId);

            if (string.IsNullOrEmpty(parentName))
            {
                query = query.Where(o => o.ParentName == null);
            }
            else
            {
                query = query.Where(o => o.ParentName == parentName);
            }

            query = query.OrderBy(o => o.ShowOrder);

            return query.ToArray();
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<UserGroup> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoUg.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = LocaleHelper.Localizer.Get("SaveFailure");
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        public IEnumerable<UserGroupUser> Users(string group)
        {
            var query = repoUgu.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.UserGroupName == group);
            return query.ToArray();
        }

        [HttpPost]
        public ServerResponse SaveUsers(string group, [FromBody]UserGroupUser[] objs)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                trans.BeginTransaction();
                repoUgu.DeleteBatch(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.UserGroupName == group);
                if (objs != null)
                {
                    foreach (var obj in objs)
                    {
                        repoUgu.Insert(obj);
                    }
                }
                trans.Commit();
                response.Status = ResponseStatus.Success;
                response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}