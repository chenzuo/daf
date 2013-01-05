namespace DAF.SSO.LocalProvider.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Reflection;
    using DAF.Core;
    using DAF.Core.Security;
    using DAF.Core.Serialization;
    using DAF.Core.Serialization.JsonNet;
    using DAF.Web;
    using DAF.SSO.Server;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.SSO.LocalProvider.DB.SSODB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.SSO.LocalProvider.DB.SSODB context)
        {
            var encrypt = new HashEncryptionProvider();

            var role = new Role
            {
                RoleId = "admin",
                RoleName = "admin",
                ClientId = null,
                Status = DataStatus.Normal
            };

            context.Roles.AddOrUpdate(r => new { r.RoleId }, role);

            var user = new User
            {
                UserId = "admin",
                ClientId = null,
                Account = "admin",
                Password = encrypt.Encrypt("123456"),
                Email = "admin@admin.com",
                NickName = "admin",
                FullName = "administrator",
                Sex = Sex.Male,
                Status = DataStatus.Normal,
                Theme = "Default",
                Skin = "Default",
                Locale = "zh-CN",
                TimeZone = 8.0,
            };

            context.Users.AddOrUpdate(u => u.UserId, user);
            context.SaveChanges();

            context.UserRoles.AddOrUpdate(ur => new { ur.UserId, ur.RoleId },
                new UserRole { User = user, Role = role }
                );
            context.SaveChanges();
        }
    }
}
