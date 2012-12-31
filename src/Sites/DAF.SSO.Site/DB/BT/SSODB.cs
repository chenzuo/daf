using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;
using DAF.Core.Data.BLToolkit;

namespace DAF.SSO.Site.DB.BT
{
    public class SSODB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "SSODB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(User),
                    typeof(Role),
                    typeof(UserRole),
                    typeof(Permission),
                    typeof(RolePermission),
                    typeof(ServerSession)
                };
            }
        }
    }
}