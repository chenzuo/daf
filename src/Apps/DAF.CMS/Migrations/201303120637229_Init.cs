namespace DAF.CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cms_AppSetting",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(maxLength: 50),
                        Value = c.String(maxLength: 2000),
                        ShowOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteId, t.Category, t.Name })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.cms_SubSite",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        SiteName = c.String(nullable: false, maxLength: 50),
                        SubSiteName = c.String(nullable: false, maxLength: 50),
                        Language = c.String(maxLength: 50),
                        DateTimeFormat = c.String(maxLength: 50),
                        DateFormat = c.String(maxLength: 50),
                        TimeFormat = c.String(maxLength: 50),
                        CurrencyFormat = c.String(maxLength: 50),
                        NumberFormat = c.String(maxLength: 50),
                        TimeZone = c.Double(nullable: false),
                        DefaultTheme = c.String(maxLength: 50),
                        DefaultSkin = c.String(maxLength: 50),
                        DefaultPageTitle = c.String(maxLength: 50),
                        DefaultMetaKeywords = c.String(maxLength: 500),
                        DefaultMetaDescription = c.String(maxLength: 500),
                        HomePageId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.cms_WebSite", t => t.SiteName, cascadeDelete: true)
                .ForeignKey("dbo.cms_WebPage", t => t.HomePageId)
                .Index(t => t.SiteName)
                .Index(t => t.HomePageId);
            
            CreateTable(
                "dbo.cms_WebSite",
                c => new
                    {
                        SiteName = c.String(nullable: false, maxLength: 50),
                        UrlStartWith = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.SiteName);
            
            CreateTable(
                "dbo.cms_WebPage",
                c => new
                    {
                        PageId = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        TemplateName = c.String(nullable: false, maxLength: 50),
                        CategoryId = c.String(maxLength: 50),
                        ShortUrl = c.String(maxLength: 200),
                        HtmlUrl = c.String(maxLength: 200),
                        MetaKeywords = c.String(maxLength: 200),
                        MetaDescription = c.String(maxLength: 200),
                        PageTitle = c.String(maxLength: 50),
                        HeaderTitle = c.String(maxLength: 50),
                        PageLinks = c.String(),
                        PageCSS = c.String(),
                        PageJS = c.String(),
                        Status = c.Int(nullable: false),
                        ParentPageId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PageId)
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_PageTemplate", t => new { t.SiteId, t.TemplateName }, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => new { t.SiteId, t.TemplateName });
            
            CreateTable(
                "dbo.cms_PageTemplate",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        TemplateName = c.String(nullable: false, maxLength: 50),
                        TemplatePath = c.String(maxLength: 200),
                        AllowContentTypes = c.String(maxLength: 500),
                        PageLinks = c.String(),
                        PageCSS = c.String(),
                        PageJS = c.String(),
                    })
                .PrimaryKey(t => new { t.SiteId, t.TemplateName })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: false)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.cms_PageTemplateControl",
                c => new
                    {
                        TemplateControlId = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        TemplateName = c.String(nullable: false, maxLength: 50),
                        Section = c.String(nullable: false, maxLength: 50),
                        ControlPath = c.String(nullable: false, maxLength: 200),
                        ControlParas = c.String(),
                        Container = c.String(maxLength: 50),
                        CssStyle = c.String(),
                        ShowOrder = c.Int(nullable: false),
                        Cached = c.Boolean(nullable: false),
                        CacheKey = c.String(maxLength: 50),
                        CacheMunites = c.Int(),
                    })
                .PrimaryKey(t => t.TemplateControlId)
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_PageTemplate", t => new { t.SiteId, t.TemplateName }, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => new { t.SiteId, t.TemplateName });
            
            CreateTable(
                "dbo.cms_WebPageControl",
                c => new
                    {
                        ControlId = c.String(nullable: false, maxLength: 50),
                        PageId = c.String(nullable: false, maxLength: 50),
                        TemplateName = c.String(nullable: false, maxLength: 50),
                        Section = c.String(nullable: false, maxLength: 50),
                        ControlPath = c.String(nullable: false, maxLength: 200),
                        ControlParas = c.String(),
                        Container = c.String(maxLength: 50),
                        CssStyle = c.String(),
                        ShowOrder = c.Int(nullable: false),
                        Cached = c.Boolean(nullable: false),
                        CacheKey = c.String(maxLength: 50),
                        CacheMunites = c.Int(),
                    })
                .PrimaryKey(t => t.ControlId)
                .ForeignKey("dbo.cms_WebPage", t => t.PageId, cascadeDelete: true)
                .Index(t => t.PageId);
            
            CreateTable(
                "dbo.cms_BasicData",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(maxLength: 50),
                        Value = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        ParentId = c.String(maxLength: 50),
                        FlatId = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_BasicData", t => t.ParentId)
                .Index(t => t.SiteId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.cms_Category",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ParentId = c.String(maxLength: 50),
                        FlatId = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_Category", t => t.ParentId)
                .Index(t => t.SiteId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.cms_CategoryContent",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        CategoryId = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        TopIndex = c.Int(),
                        HotIndex = c.Int(),
                        PublishTime = c.DateTime(nullable: false),
                        OnTime = c.DateTime(),
                        OffTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.SiteId, t.CategoryId, t.ContentId })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_Category", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.cms_Content", t => new { t.SiteId, t.ContentId }, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.CategoryId)
                .Index(t => new { t.SiteId, t.ContentId });
            
            CreateTable(
                "dbo.cms_Content",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        ContentType = c.Int(nullable: false),
                        Title = c.String(maxLength: 100),
                        Keywords = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        ImageUrl = c.String(maxLength: 200),
                        ContentUrl = c.String(maxLength: 200),
                        LinkUrl = c.String(maxLength: 200),
                        ShortUrl = c.String(maxLength: 200),
                        PlainBody = c.String(),
                        HtmlBody = c.String(),
                        Properties = c.String(),
                        ContentSize = c.Double(),
                        Published = c.Boolean(nullable: false),
                        ReadCount = c.Int(nullable: false),
                        CreateAsRelated = c.Boolean(nullable: false),
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
                .PrimaryKey(t => new { t.SiteId, t.ContentId })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: false)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.cms_ContentRelation",
                c => new
                    {
                        RelationId = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        ContentId = c.String(nullable: false, maxLength: 50),
                        RelatedContentId = c.String(nullable: false, maxLength: 50),
                        RelationType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RelationId)
                .ForeignKey("dbo.cms_Content", t => new { t.SiteId, t.ContentId }, cascadeDelete: true)
                .ForeignKey("dbo.cms_Content", t => new { t.SiteId, t.RelatedContentId }, cascadeDelete: false)
                .Index(t => new { t.SiteId, t.ContentId })
                .Index(t => new { t.SiteId, t.RelatedContentId });
            
            CreateTable(
                "dbo.cms_CategoryUserGroup",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 50),
                        UserGroupId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.UserGroupId })
                .ForeignKey("dbo.cms_Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.cms_UserGroup", t => t.UserGroupId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.UserGroupId);
            
            CreateTable(
                "dbo.cms_UserGroup",
                c => new
                    {
                        UserGroupId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        SiteId = c.String(nullable: false, maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                        ParentId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UserGroupId)
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: false)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.cms_UserGroupUser",
                c => new
                    {
                        UserGroupId = c.String(nullable: false, maxLength: 50),
                        UserId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.UserGroupId, t.UserId })
                .ForeignKey("dbo.cms_UserGroup", t => t.UserGroupId, cascadeDelete: true)
                .Index(t => t.UserGroupId);
            
            CreateTable(
                "dbo.cms_MenuGroup",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.SiteId, t.Name })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.cms_MenuItem",
                c => new
                    {
                        SiteId = c.String(nullable: false, maxLength: 50),
                        MenuGroupName = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Caption = c.String(nullable: false, maxLength: 50),
                        Icon = c.String(maxLength: 200),
                        Shortcut = c.String(maxLength: 50),
                        Tooltip = c.String(maxLength: 50),
                        LinkUrl = c.String(maxLength: 200),
                        ProtectedUri = c.String(maxLength: 200),
                        Target = c.String(maxLength: 50),
                        ItemType = c.Int(nullable: false),
                        ParentName = c.String(maxLength: 50),
                        ShowOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteId, t.MenuGroupName, t.Name })
                .ForeignKey("dbo.cms_SubSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.cms_MenuGroup", t => new { t.SiteId, t.MenuGroupName }, cascadeDelete: false)
                .ForeignKey("dbo.cms_MenuItem", t => new { t.SiteId, t.MenuGroupName, t.ParentName })
                .Index(t => t.SiteId)
                .Index(t => new { t.SiteId, t.MenuGroupName })
                .Index(t => new { t.SiteId, t.MenuGroupName, t.ParentName });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.cms_MenuItem", new[] { "SiteId", "MenuGroupName", "ParentName" });
            DropIndex("dbo.cms_MenuItem", new[] { "SiteId", "MenuGroupName" });
            DropIndex("dbo.cms_MenuItem", new[] { "SiteId" });
            DropIndex("dbo.cms_MenuGroup", new[] { "SiteId" });
            DropIndex("dbo.cms_UserGroupUser", new[] { "UserGroupId" });
            DropIndex("dbo.cms_UserGroup", new[] { "SiteId" });
            DropIndex("dbo.cms_CategoryUserGroup", new[] { "UserGroupId" });
            DropIndex("dbo.cms_CategoryUserGroup", new[] { "CategoryId" });
            DropIndex("dbo.cms_ContentRelation", new[] { "Content_SiteId", "Content_ContentId" });
            DropIndex("dbo.cms_ContentRelation", new[] { "SiteId", "RelatedContentId" });
            DropIndex("dbo.cms_ContentRelation", new[] { "SiteId", "ContentId" });
            DropIndex("dbo.cms_Content", new[] { "SiteId" });
            DropIndex("dbo.cms_CategoryContent", new[] { "SiteId", "ContentId" });
            DropIndex("dbo.cms_CategoryContent", new[] { "CategoryId" });
            DropIndex("dbo.cms_CategoryContent", new[] { "SiteId" });
            DropIndex("dbo.cms_Category", new[] { "ParentId" });
            DropIndex("dbo.cms_Category", new[] { "SiteId" });
            DropIndex("dbo.cms_BasicData", new[] { "ParentId" });
            DropIndex("dbo.cms_BasicData", new[] { "SiteId" });
            DropIndex("dbo.cms_WebPageControl", new[] { "PageId" });
            DropIndex("dbo.cms_PageTemplateControl", new[] { "SiteId", "TemplateName" });
            DropIndex("dbo.cms_PageTemplateControl", new[] { "SiteId" });
            DropIndex("dbo.cms_PageTemplate", new[] { "SiteId" });
            DropIndex("dbo.cms_WebPage", new[] { "SubSite_SiteId" });
            DropIndex("dbo.cms_WebPage", new[] { "SiteId", "TemplateName" });
            DropIndex("dbo.cms_WebPage", new[] { "SiteId" });
            DropIndex("dbo.cms_SubSite", new[] { "HomePageId" });
            DropIndex("dbo.cms_SubSite", new[] { "SiteName" });
            DropIndex("dbo.cms_AppSetting", new[] { "SiteId" });
            DropForeignKey("dbo.cms_MenuItem", new[] { "SiteId", "MenuGroupName", "ParentName" }, "dbo.cms_MenuItem");
            DropForeignKey("dbo.cms_MenuItem", new[] { "SiteId", "MenuGroupName" }, "dbo.cms_MenuGroup");
            DropForeignKey("dbo.cms_MenuItem", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_MenuGroup", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_UserGroupUser", "UserGroupId", "dbo.cms_UserGroup");
            DropForeignKey("dbo.cms_UserGroup", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_CategoryUserGroup", "UserGroupId", "dbo.cms_UserGroup");
            DropForeignKey("dbo.cms_CategoryUserGroup", "CategoryId", "dbo.cms_Category");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "Content_SiteId", "Content_ContentId" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "SiteId", "RelatedContentId" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_ContentRelation", new[] { "SiteId", "ContentId" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_Content", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_CategoryContent", new[] { "SiteId", "ContentId" }, "dbo.cms_Content");
            DropForeignKey("dbo.cms_CategoryContent", "CategoryId", "dbo.cms_Category");
            DropForeignKey("dbo.cms_CategoryContent", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_Category", "ParentId", "dbo.cms_Category");
            DropForeignKey("dbo.cms_Category", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_BasicData", "ParentId", "dbo.cms_BasicData");
            DropForeignKey("dbo.cms_BasicData", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_WebPageControl", "PageId", "dbo.cms_WebPage");
            DropForeignKey("dbo.cms_PageTemplateControl", new[] { "SiteId", "TemplateName" }, "dbo.cms_PageTemplate");
            DropForeignKey("dbo.cms_PageTemplateControl", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_PageTemplate", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_WebPage", "SubSite_SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_WebPage", new[] { "SiteId", "TemplateName" }, "dbo.cms_PageTemplate");
            DropForeignKey("dbo.cms_WebPage", "SiteId", "dbo.cms_SubSite");
            DropForeignKey("dbo.cms_SubSite", "HomePageId", "dbo.cms_WebPage");
            DropForeignKey("dbo.cms_SubSite", "SiteName", "dbo.cms_WebSite");
            DropForeignKey("dbo.cms_AppSetting", "SiteId", "dbo.cms_SubSite");
            DropTable("dbo.cms_MenuItem");
            DropTable("dbo.cms_MenuGroup");
            DropTable("dbo.cms_UserGroupUser");
            DropTable("dbo.cms_UserGroup");
            DropTable("dbo.cms_CategoryUserGroup");
            DropTable("dbo.cms_ContentRelation");
            DropTable("dbo.cms_Content");
            DropTable("dbo.cms_CategoryContent");
            DropTable("dbo.cms_Category");
            DropTable("dbo.cms_BasicData");
            DropTable("dbo.cms_WebPageControl");
            DropTable("dbo.cms_PageTemplateControl");
            DropTable("dbo.cms_PageTemplate");
            DropTable("dbo.cms_WebPage");
            DropTable("dbo.cms_WebSite");
            DropTable("dbo.cms_SubSite");
            DropTable("dbo.cms_AppSetting");
        }
    }
}
