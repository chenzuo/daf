namespace DAF.SSO.Server.Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sso_User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 50),
                        Account = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(maxLength: 50),
                        SecurityCode = c.String(maxLength: 256),
                        NickName = c.String(maxLength: 50),
                        FullName = c.String(maxLength: 50),
                        Sex = c.Int(),
                        Birthday = c.DateTime(),
                        Email = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Locale = c.String(maxLength: 10),
                        TimeZone = c.Double(nullable: false),
                        Theme = c.String(maxLength: 20),
                        Skin = c.String(maxLength: 20),
                        Avatar = c.String(maxLength: 200),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.sso_UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 50),
                        RoleId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.sso_User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.sso_Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.sso_Role",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(maxLength: 50),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        ActiveTime = c.DateTime(),
                        ExpiryTime = c.DateTime(),
                        Status = c.Int(nullable: false),
                        ParentRoleId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.sso_Role", t => t.ParentRoleId)
                .Index(t => t.ParentRoleId);
            
            CreateTable(
                "dbo.sso_RolePermission",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        PermissionType = c.Int(nullable: false),
                        Permissions = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ClientId, t.PermissionType })
                .ForeignKey("dbo.sso_Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.sso_Permission",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 50),
                        PermissionName = c.String(nullable: false, maxLength: 50),
                        PermissionType = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Uri = c.String(nullable: false, maxLength: 200),
                        GroupName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.ClientId, t.PermissionName, t.PermissionType });
            
            CreateTable(
                "dbo.sso_ServerSession",
                c => new
                    {
                        SessionId = c.String(nullable: false, maxLength: 128),
                        CientId = c.String(nullable: false, maxLength: 50),
                        FromCientId = c.String(maxLength: 50),
                        FromSessionId = c.String(maxLength: 50),
                        DeviceId = c.String(maxLength: 50),
                        DeviceInfo = c.String(maxLength: 500),
                        UserId = c.String(maxLength: 50),
                        AccessToken = c.String(maxLength: 50),
                        AccessTokenExpiryTime = c.DateTime(),
                        LastAccessTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.SessionId, t.CientId });

            CreateIndex("dbo.sso_User", "Account", true);
            CreateIndex("dbo.sso_User", "Email", false);
            CreateIndex("dbo.sso_User", "Mobile", false);
            CreateIndex("dbo.sso_Role", "RoleName", true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.sso_Role", new[] { "RoleName" });
            DropIndex("dbo.sso_User", new[] { "Mobile" });
            DropIndex("dbo.sso_User", new[] { "Email" });
            DropIndex("dbo.sso_User", new[] { "Account" });
            
            DropIndex("dbo.sso_RolePermission", new[] { "RoleId" });
            DropIndex("dbo.sso_Role", new[] { "ParentRoleId" });
            DropIndex("dbo.sso_UserRole", new[] { "RoleId" });
            DropIndex("dbo.sso_UserRole", new[] { "UserId" });
            DropForeignKey("dbo.sso_RolePermission", "RoleId", "dbo.sso_Role");
            DropForeignKey("dbo.sso_Role", "ParentRoleId", "dbo.sso_Role");
            DropForeignKey("dbo.sso_UserRole", "RoleId", "dbo.sso_Role");
            DropForeignKey("dbo.sso_UserRole", "UserId", "dbo.sso_User");
            DropTable("dbo.sso_ServerSession");
            DropTable("dbo.sso_Permission");
            DropTable("dbo.sso_RolePermission");
            DropTable("dbo.sso_Role");
            DropTable("dbo.sso_UserRole");
            DropTable("dbo.sso_User");
        }
    }
}
