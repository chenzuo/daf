using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Security;
using DAF.Core.Localization;
using DAF.Web;
using DAF.Web.Security;
using DAF.SSO;
using DAF.SSO.Server;
using DAF.SSO.Site.ViewModels;

namespace DAF.SSO.Site.Api
{
    public class PermissionController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<Permission> repoPermission;

        public PermissionController(ITransactionManager trans, 
            IRepository<Permission> repoPermission)
        {
            this.trans = trans;
            this.repoPermission = repoPermission;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Permission> Get(string client, int pi = 1, int ps = 50)
        {
            IQueryable<Permission> query = repoPermission.Query(null);
            if (!string.IsNullOrEmpty(client))
            {
                client = client.ToLower();
                query = query.Where(o => o.ClientId == client);
            }
            query = query.OrderBy(o => o.ClientId).ThenBy(o => o.Position);
            if (pi >= 0 && ps > 0)
            {
                query = query.Paginate(pi, ps);
            }
            var objs = query.ToArray();
            return objs;
        }

        [HttpPost]
        public ServerResponse Post([FromBody]ChangedData<Permission> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoPermission.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
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
    }
}