namespace DAF.SSO.Server.Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tl_TimelineItem",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        EventType = c.String(nullable: false, maxLength: 50),
                        EventName = c.String(maxLength: 50),
                        UserId = c.String(nullable: false, maxLength: 50),
                        UserType = c.String(maxLength: 10),
                        UserName = c.String(maxLength: 50),
                        Title = c.String(maxLength: 50),
                        Decription = c.String(maxLength: 200),
                        ImageUrl = c.String(maxLength: 200),
                        DetailUrl = c.String(maxLength: 200),
                        LinkUrl = c.String(maxLength: 200),
                        UserUrl = c.String(maxLength: 200),
                        SiteName = c.String(maxLength: 50),
                        SiteUrl = c.String(maxLength: 200),
                        ActionTime = c.DateTime(nullable: false),
                        Keywords = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.tl_TimelineItemHistory",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.tl_TimelineItem", t => t.ItemId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.tl_TimelineItemHistory", new[] { "ItemId" });
            DropForeignKey("dbo.tl_TimelineItemHistory", "ItemId", "dbo.tl_TimelineItem");
            DropTable("dbo.tl_TimelineItemHistory");
            DropTable("dbo.tl_TimelineItem");
        }
    }
}
