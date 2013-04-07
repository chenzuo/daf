namespace DAF.CMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DAF.CMS.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.CMS.DB.EF.CMSDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.CMS.DB.EF.CMSDB context)
        {
            WebSite site = new WebSite()
            {
                SiteName = "CMS",
                UrlStartWith = "http://www.cms.com"
            };
            context.WebSites.Add(site);
            context.SaveChanges();

            SubSite cnSite = new SubSite()
            {
                SiteId = "1",
                SiteName = "CMS",
                SubSiteName = "zh-cn",
                Language = "zh-CN",
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss",
                DateFormat = "yyyy-MM-dd",
                TimeFormat = "HH:mm",
                CurrencyFormat = "£§{0:00}",
                NumberFormat = "{0:00}",
                TimeZone = 8.0d,
                DefaultTheme = "Default",
                DefaultSkin = "Default",
                DefaultPageTitle = "≤‚ ‘Õ¯’æ",
                DefaultMetaKeywords = "≤‚ ‘Õ¯’æ,CMS,◊‘÷˙Ω®’æ",
                DefaultMetaDescription = "◊‘÷˙Ω®’æœµÕ≥"
            };
            context.LocaleSites.Add(cnSite);
            SubSite enSite = new SubSite()
            {
                SiteId = "2",
                SiteName = "CMS",
                SubSiteName = "en-us",
                Language = "en-US",
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss",
                DateFormat = "yyyy-MM-dd",
                TimeFormat = "HH:mm",
                CurrencyFormat = "${0:00}",
                NumberFormat = "{0:00}",
                TimeZone = -8.0d,
                DefaultTheme = "Default",
                DefaultSkin = "Default",
                DefaultPageTitle = "Test Site",
                DefaultMetaKeywords = "Test Site,CMS,Site DIY",
                DefaultMetaDescription = "Site DIY"
            };
            context.LocaleSites.Add(enSite);

            context.SaveChanges();
        }
    }
}
