using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAF.CMS.Models;

namespace DAF.CMS.DB.EF
{
    public class CMSDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<BasicDataItem> BasicDataItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<CategoryContent> CategoryContents { get; set; }
        public DbSet<ContentRelation> ContentRelations { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupUser> UserGroupUsers { get; set; }
        public DbSet<CategoryUserGroup> CategoryUserGroups { get; set; }
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<SubSite> LocaleSites { get; set; }
        public DbSet<PageTemplate> PageTemplates { get; set; }
        public DbSet<PageTemplateControl> PageTemplateControls { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        public DbSet<WebPageControl> WebPageControls { get; set; }
        public DbSet<SiteMenuGroup> MenuGroups { get; set; }
        public DbSet<SiteMenuItem> MenuItems { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer<CMSDB>(null);
        }

        public int ExecuteOrder
        {
            get { return Core.ExecuteOrder.Later; }
        }
    }
}