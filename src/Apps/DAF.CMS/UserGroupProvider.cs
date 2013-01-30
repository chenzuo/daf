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
    public class UserGroupProvider : IUserGroupProvider
    {
        private ITransactionManager trans;
        private IRepository<UserGroup> repoUg;
        private IRepository<UserGroupUser> repoUgu;

        public UserGroupProvider(ITransactionManager trans, IRepository<UserGroup> repoUg, IRepository<UserGroupUser> repoUgu)
        {
            this.trans = trans;
            this.repoUg = repoUg;
            this.repoUgu = repoUgu;
        }

        public IEnumerable<UserGroup> Query(string siteId, string parentId = null)
        {
            var query = repoUg.Query(o => o.SiteId == siteId);

            if (string.IsNullOrEmpty(parentId))
            {
                query = query.Where(o => o.ParentId == null);
            }
            else
            {
                query = query.Where(o => o.ParentId == parentId);
            }

            query = query.OrderBy(o => o.ShowOrder);

            return query.ToArray();
        }

        public bool Save(ChangedData<UserGroup> items)
        {
            return repoUg.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }

        public IEnumerable<UserGroupUser> GetGroupUsers(string group)
        {
            var query = repoUgu.Query(o => o.UserGroupId == group);
            return query.ToArray();
        }

        public bool SaveGroupUsers(string group, UserGroupUser[] objs)
        {
            try
            {
                trans.BeginTransaction();
                repoUgu.DeleteBatch(o => o.UserGroupId == group);
                if (objs != null)
                {
                    foreach (var obj in objs)
                    {
                        repoUgu.Insert(obj);
                    }
                }
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }
}
