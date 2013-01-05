using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;

namespace DAF.SSO.LocalProvider.DB
{
    public class SSODB : DbContext, DAF.Core.IStartup
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ServerSession> ServerSessions { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer<SSODB>(null);
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Later; }
        }
    }
}