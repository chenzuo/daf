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
    public class UserController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<User> repoUser;
        private IRepository<UserRole> repoUr;
        private IRepository<Role> repoRole;

        public UserController(ITransactionManager trans, IRepository<User> repoUser, IRepository<Role> repoRole, IRepository<UserRole> repoUr)
            : base()
        {
            this.trans = trans;
            this.repoUser = repoUser;
            this.repoRole = repoRole;
            this.repoUr = repoUr;
        }

        [HttpGet]
        //[DAF.Web.Api.Filters.OAuthorize]
        public IEnumerable<User> Get(string startWith = null, int pi = 1, int ps = 50)
        {
            var query = repoUser.Query(null);
            if (!string.IsNullOrEmpty(startWith))
            {
                startWith = startWith.ToLower();
                query = query.Where(o => o.Account.StartsWith(startWith));
            }
            if (pi >= 0 && ps > 0)
            {
                query = query.Paginate(pi, ps);
            }

            return query;
        }

        [HttpGet]
        public User Get(string id)
        {
            var user = repoUser.Query(o => o.UserId == id).FirstOrDefault();
            return user;
        }

        [HttpPost]
        public ServerResponse Post([FromBody]ChangedData<User> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoUser.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems,
                    null, delegate(EntityEventArgs<User> args) {
                        if(args.Entity.UserRoles != null && args.Entity.UserRoles.Count > 0)
                        {
                            args.Entity.UserRoles.ForEach(ur => repoUr.Insert(ur));
                        }
                    }, null, delegate(EntityEventArgs<User> args) {
                        repoUr.DeleteBatch(ur => ur.UserId == args.Entity.UserId);
                        if(args.Entity.UserRoles != null && args.Entity.UserRoles.Count > 0)
                        {
                            args.Entity.UserRoles.ForEach(ur => repoUr.Insert(ur));
                        }
                    }, delegate(EntityEventArgs<User> args) {
                        repoUr.DeleteBatch(ur => ur.UserId == args.Entity.UserId);
                    }, null))
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
        public IEnumerable<UserRoleInfo> Roles(string id)
        {
            var roles = repoRole.Query(o => o.Status != DataStatus.Deleted).ToArray();
            var urs = repoUr.Query(o => o.UserId == id).ToArray();
            var objs = roles.Select(o => new UserRoleInfo()
            {
                RoleId = o.RoleId,
                RoleName = o.RoleName,
                IsSelected = urs.Any(ur => ur.RoleId == o.RoleId)
            });
            return objs;
        }

        [HttpPost]
        public ServerResponse Roles(string id, [FromBody]UserRoleInfo[] roles)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                trans.BeginTransaction();
                repoUr.DeleteBatch(o => o.UserId == id);
                if (roles != null)
                {
                    foreach (var ur in roles)
                    {
                        if (ur.IsSelected)
                            repoUr.Insert(new UserRole() { UserId = id, RoleId = ur.RoleId });
                    }
                }
                trans.Commit();
                response.Status = ResponseStatus.Success;
                response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
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