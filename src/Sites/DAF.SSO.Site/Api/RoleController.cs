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
    public class RoleController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<Role> repoRole;
        private IRepository<RolePermission> repoRolePermission;
        private IRepository<Permission> repoPermission;

        public RoleController(ITransactionManager trans, 
            IRepository<Role> repoRole, IRepository<RolePermission> repoRolePermission, IRepository<Permission> repoPermission)
            : base()
        {
            this.trans = trans;
            this.repoRole = repoRole;
            this.repoRolePermission = repoRolePermission;
            this.repoPermission = repoPermission;
        }

        [HttpGet]
        public IEnumerable<Role> Get(string client = null, string parent = null)
        {
            var query = repoRole.Query(null);
            if (!string.IsNullOrEmpty(client))
                query = query.Where(o => o.ClientId == client);
            if (string.IsNullOrEmpty(parent))
                query = query.Where(o => o.ParentRoleId == null);
            else
                query = query.Where(o => o.ParentRoleId == parent);

            var objs = query.ToArray();
            objs.ForEach(o =>
            {
                o.Children = new List<Role>();
            });

            return objs;
        }

        [HttpPost]
        public ServerResponse Post([FromBody]ChangedData<Role> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoRole.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
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
        public IEnumerable<PermissionInfo> Permissions(string client, string role)
        {
            client = client.ToLower();
            var ps = repoPermission.Query(o => o.ClientId == client).ToArray();
            var rps = repoRolePermission.Query(o => o.ClientId == client && o.RoleId == role);
            var rpis = ps.Select(o => new PermissionInfo()
            {
                PermissionName = o.PermissionName,
                PermissionType = o.PermissionType,
                Position = o.Position,
                GroupName = o.GroupName,
                Uri = o.Uri,
                HasPermitted = rps.HasPermitted(o)
            });

            return rpis;
        }

        [HttpPost]
        public ServerResponse Permissions(string client, string role, [FromBody]PermissionInfo[] rpis)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                trans.BeginTransaction();
                repoRolePermission.DeleteBatch(o => o.ClientId == client && o.RoleId == role);

                if (rpis != null && rpis.Length > 0)
                {
                    var pts = rpis.Select(o => o.PermissionType).Distinct();
                    foreach (var pt in pts)
                    {
                        var prpis = rpis.Where(o => o.PermissionType == pt);
                        int maxIdx = prpis.Select(o => o.Position).Max();
                        char[] ps = new char[maxIdx + 1];
                        foreach (var rpi in prpis)
                        {
                            ps[rpi.Position] = rpi.HasPermitted ? '1' : '0';
                        }
                        var rp = new RolePermission()
                        {
                            RoleId = role,
                            PermissionType = pt,
                            ClientId = client,
                            Permissions = new string(ps)
                        };
                        repoRolePermission.Insert(rp);
                    }
                }
                response.Status = ResponseStatus.Success;
                response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
                trans.Commit();
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