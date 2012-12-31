using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAF.Web.Menu;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.DB.EF
{
    public class CMSDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<Content> Contents { get; set; }
        public DbSet<CategoryContent> CategoryContents { get; set; }
        public DbSet<ContentRelation> ContentRelations { get; set; }
        public DbSet<BasicDataItem> BasicDataItems { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupUser> UserGroupUsers { get; set; }
        public DbSet<CategoryUserGroup> CategoryUserGroups { get; set; }
        public DbSet<MenuGroup> MenuGroups { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

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