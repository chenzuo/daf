using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAF.Social.Models;

namespace DAF.Social.LocalProvider.DB.EF
{
    public class SocialDB : DbContext, DAF.Core.IStartup
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Org> Orgs { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }
        public DbSet<PersonLink> PersonLinks { get; set; }
        public DbSet<PersonCertificate> PersonCertificates { get; set; }
        public DbSet<WorkResume> WorkResumes { get; set; }
        public DbSet<StudyResume> StudyResumes { get; set; }

        public void OnStarted()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SocialDB, Migrations.Configuration>());
        }

        public int ExecuteOrder
        {
            get { return DAF.Core.ExecuteOrder.Later; }
        }
    }
}