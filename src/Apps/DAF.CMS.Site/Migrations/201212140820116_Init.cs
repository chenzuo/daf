namespace DAF.CMS.Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cms_Content",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        Language = c.String(nullable: false, maxLength: 10),
                        Title = c.String(maxLength: 100),
                        Keywords = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        ImageUrl = c.String(maxLength: 200),
                        ContentUrl = c.String(maxLength: 200),
                        LinkUrl = c.String(maxLength: 200),
                        ShortUrl = c.String(maxLength: 200),
                        PlainBody = c.String(),
                        HtmlBody = c.String(),
                        ContentType = c.Int(nullable: false),
                        Properties = c.String(),
                        ContentSize = c.Double(),
                        Published = c.Boolean(nullable: false),
                        ReadCount = c.Int(nullable: false),
                        CreatorId = c.String(maxLength: 50),
                        CreatorName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        ModifierId = c.String(maxLength: 50),
                        ModifierName = c.String(maxLength: 50),
                        ModifiedTime = c.DateTime(),
                        PublisherId = c.String(maxLength: 50),
                        PublisherName = c.String(maxLength: 50),
                        PublishTime = c.DateTime(),
                        ShowOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteName, t.ContentId, t.Language });
            
            CreateTable(
                "dbo.cms_CategoryContent",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        CategoryCode = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        Language = c.String(nullable: false, maxLength: 10),
                        TopIndex = c.Int(),
                        HotIndex = c.Int(),
                        PublishTime = c.DateTime(nullable: false),
                        OnTime = c.DateTime(),
                        OffTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.SiteName, t.CategoryCode, t.ContentId, t.Language })
                .ForeignKey("dbo.cms_Content", t => new { t.SiteName, t.ContentId, t.Language }, cascadeDelete: true)
                .Index(t => new { t.SiteName, t.ContentId, t.Language });
            
            CreateTable(
                "dbo.cms_ContentRelation",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        RelatedContentId = c.String(nullable: false, maxLength: 50),
                        Language = c.String(nullable: false, maxLength: 10),
                        RelationType = c.String(nullable: false, maxLength: 50)
                    })
                .PrimaryKey(t => new { t.SiteName, t.ContentId, t.RelatedContentId, t.Language, t.RelationType })
                .ForeignKey("dbo.cms_Content", t => new { t.SiteName, t.ContentId, t.Language }, cascadeDelete: false)
                .ForeignKey("dbo.cms_Content", t => new { t.SiteName, t.RelatedContentId, t.Language }, cascadeDelete: false)
                .Index(t => new { t.SiteName, t.ContentId, t.Language })
                .Index(t => new { t.SiteName, t.RelatedContentId, t.Language });
            
            CreateTable(
                "dbo.sys_BasicData",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(maxLength: 50),
                        Value = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        ParentName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteName, t.ClientId, t.Category, t.Name })
                .ForeignKey("dbo.sys_BasicData", t => new { t.SiteName, t.ClientId, t.Category, t.ParentName })
                .Index(t => new { t.SiteName, t.ClientId, t.Category, t.ParentName });
            
            CreateTable(
                "dbo.sys_AppSetting",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(maxLength: 50),
                        Value = c.String(maxLength: 2000),
                        ShowOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteName, t.ClientId, t.Category, t.Name });
            
            CreateTable(
                "dbo.sys_Category",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        Language = c.String(nullable: false, maxLength: 10),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ParentCode = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteName, t.Language, t.Code })
                .ForeignKey("dbo.sys_Category", t => new { t.SiteName, t.Language, t.ParentCode })
                .Index(t => new { t.SiteName, t.Language, t.ParentCode });
            
            CreateTable(
                "dbo.sys_CategoryUserGroup",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        Language = c.String(nullable: false, maxLength: 10),
                        CategoryCode = c.String(nullable: false, maxLength: 50),
                        UserGroupName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteName, t.Language, t.CategoryCode, t.UserGroupName })
                .ForeignKey("dbo.sys_Category", t => new { t.SiteName, t.Language, t.CategoryCode }, cascadeDelete: true)
                .ForeignKey("dbo.sys_UserGroup", t => new { t.SiteName, t.UserGroupName }, cascadeDelete: true)
                .Index(t => new { t.SiteName, t.Language, t.CategoryCode })
                .Index(t => new { t.SiteName, t.UserGroupName });
            
            CreateTable(
                "dbo.sys_UserGroup",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        ParentName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteName, t.Name })
                .ForeignKey("dbo.sys_UserGroup", t => new { t.SiteName, t.ParentName })
                .Index(t => new { t.SiteName, t.ParentName });
            
            CreateTable(
                "dbo.sys_UserGroupUser",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        UserGroupName = c.String(nullable: false, maxLength: 50),
                        UserId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteName, t.UserGroupName, t.UserId })
                .ForeignKey("dbo.sys_UserGroup", t => new { t.SiteName, t.UserGroupName }, cascadeDelete: true)
                .Index(t => new { t.SiteName, t.UserGroupName });
            
            CreateTable(
                "dbo.daf_MenuGroup",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Caption = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.daf_MenuItem",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Caption = c.String(nullable: false),
                        Icon = c.String(maxLength: 200),
                        Shortcut = c.String(),
                        Tooltip = c.String(),
                        LinkUrl = c.String(maxLength: 200),
                        ProtectedUri = c.String(),
                        Target = c.String(),
                        ItemType = c.Int(nullable: false),
                        MenuGroupName = c.String(maxLength: 128),
                        ParentName = c.String(maxLength: 128),
                        ShowOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.daf_MenuItem", t => t.ParentName)
                .ForeignKey("dbo.daf_MenuGroup", t => t.MenuGroupName)
                .Index(t => t.ParentName)
                .Index(t => t.MenuGroupName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.daf_MenuItem", new[] { "MenuGroupName" });
            DropIndex("dbo.daf_MenuItem", new[] { "ParentName" });
            DropIndex("dbo.sys_UserGroupUser", new[] { "SiteName", "UserGroupName" });
            DropIndex("dbo.sys_UserGroup", new[] { "SiteName", "ParentName" });
            DropIndex("dbo.sys_CategoryUserGroup", new[] { "SiteName", "UserGroupName" });
            DropIndex("dbo.sys_CategoryUserGroup", new[] { "SiteName", "Language", "CategoryCode" });
            DropIndex("dbo.sys_Category", new[] { "SiteName", "Language", "ParentCode" });
            DropIndex("dbo.sys_BasicData", new[] { "SiteName", "ClientId", "Category", "ParentName" });
            DropIndex("dbo.cms_ContentRelation", new[] { "Content_SiteName", "Content_ContentId", "Content_Language" });
            DropIndex("dbo.cms_ContentRelation", new[] { "SiteName", "RelatedContentId", "Language" });
            DropIndex("dbo.cms_ContentRelation", new[] { "SiteName", "ContentId", "Language" });
            DropIndex("dbo.cms_CategoryContent", new[] { "SiteName", "ContentId", "Language" });
            DropForeignKey("dbo.daf_MenuItem", "MenuGroupName", "dbo.daf_MenuGroup");
            DropForeignKey("dbo.daf_MenuItem", "ParentName", "dbo.daf_MenuItem");
            DropForeignKey("dbo.sys_UserGroupUser", new[] { "SiteName", "UserGroupName" }, "dbo.sys_UserGroup");
            DropForeignKey("dbo.sys_UserGroup", new[] { "SiteName", "ParentName" }, "dbo.sys_UserGroup");
            DropForeignKey("dbo.sys_CategoryUserGroup", new[] { "SiteName", "UserGroupName" }, "dbo.sys_UserGroup");
            DropForeignKey("dbo.sys_CategoryUserGroup", new[] { "SiteName", "Language", "CategoryCode" }, "dbo.sys_Category");
            DropForeignKey("dbo.sys_Category", new[] { "SiteName", "Language", "ParentCode" }, "dbo.sys_Category");
            DropForeignKey("dbo.sys_BasicData", new[] { "SiteName", "ClientId", "Category", "ParentName" }, "dbo.sys_BasicData");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "Content_SiteName", "Content_ContentId", "Content_Language" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "SiteName", "RelatedContentId", "Language" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "SiteName", "ContentId", "Language" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_CategoryContent", new[] { "SiteName", "ContentId", "Language" }, "dbo.cms_Content");
            DropTable("dbo.daf_MenuItem");
            DropTable("dbo.daf_MenuGroup");
            DropTable("dbo.sys_UserGroupUser");
            DropTable("dbo.sys_UserGroup");
            DropTable("dbo.sys_CategoryUserGroup");
            DropTable("dbo.sys_Category");
            DropTable("dbo.sys_AppSetting");
            DropTable("dbo.sys_BasicData");
            DropTable("dbo.cms_ContentRelation");
            DropTable("dbo.cms_CategoryContent");
            DropTable("dbo.cms_Content");
        }
    }
}
