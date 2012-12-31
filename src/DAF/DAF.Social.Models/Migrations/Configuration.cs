namespace DAF.Social.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAF.Social.Models.DB.EF.SocialDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAF.Social.Models.DB.EF.SocialDB context)
        {
            context.Contacts.Add(new Contact() { ContactType = "qq", ValidationRegex = @"\d+" });
            context.Contacts.Add(new Contact() { ContactType = "weibo", ValidationRegex = @"\d+" });
            context.Contacts.Add(new Contact() { ContactType = "weixin", ValidationRegex = @"\d+" });
            context.Contacts.Add(new Contact() { ContactType = "email", ValidationRegex = @"\b[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b" });
            context.Contacts.Add(new Contact() { ContactType = "phone", ValidationRegex = @"\d{6,14}" });
            context.Contacts.Add(new Contact() { ContactType = "mobile", ValidationRegex = @"\d{7,14}" });
            context.Contacts.Add(new Contact() { ContactType = "address", ValidationRegex = @"" });
            context.Contacts.Add(new Contact() { ContactType = "postcode", ValidationRegex = @"\d{6}" });

            context.SaveChanges();
        }
    }
}