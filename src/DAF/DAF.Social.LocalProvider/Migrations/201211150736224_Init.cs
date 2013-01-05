namespace DAF.Social.LocalProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sns_Contact",
                c => new
                    {
                        ContactType = c.String(nullable: false, maxLength: 50),
                        ValidationRegex = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ContactType);
            
            CreateTable(
                "dbo.sns_Org",
                c => new
                    {
                        OrgId = c.String(nullable: false, maxLength: 50),
                        OrgName = c.String(nullable: false, maxLength: 100),
                        BriefName = c.String(maxLength: 50),
                        OrgType = c.String(maxLength: 50),
                        Owner = c.String(maxLength: 50),
                        Description = c.String(maxLength: 500),
                        EstablishDate = c.DateTime(),
                        SiteUrl = c.String(maxLength: 200),
                        DetailUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.OrgId);
            
            CreateTable(
                "dbo.sns_Person",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 50),
                        PersonName = c.String(nullable: false, maxLength: 50),
                        Sex = c.Int(),
                        Birthday = c.DateTime(),
                        IDCard = c.String(maxLength: 50),
                        Photo = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.sns_PersonContact",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 50),
                        ContactType = c.String(nullable: false, maxLength: 50),
                        Detail = c.String(),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ContactType })
                .ForeignKey("dbo.sns_Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.sns_Contact", t => t.ContactType, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.ContactType);
            
            CreateTable(
                "dbo.sns_WorkResume",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 50),
                        OrgId = c.String(nullable: false, maxLength: 50),
                        JoinDate = c.DateTime(nullable: false),
                        DepartmentName = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        LeaveDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.PersonId, t.OrgId, t.JoinDate })
                .ForeignKey("dbo.sns_Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.sns_Org", t => t.OrgId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.OrgId);
            
            CreateTable(
                "dbo.sns_StudyResume",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 50),
                        SchoolId = c.String(nullable: false, maxLength: 50),
                        JoinDate = c.DateTime(nullable: false),
                        GradeName = c.String(maxLength: 50),
                        ClassName = c.String(maxLength: 50),
                        SubjectName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        LeaveDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.PersonId, t.SchoolId, t.JoinDate })
                .ForeignKey("dbo.sns_Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.sns_School", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.sns_School",
                c => new
                    {
                        SchoolId = c.String(nullable: false, maxLength: 50),
                        SchoolName = c.String(nullable: false, maxLength: 100),
                        BriefName = c.String(maxLength: 50),
                        Owner = c.String(maxLength: 50),
                        Description = c.String(maxLength: 500),
                        EstablishDate = c.DateTime(),
                        SiteUrl = c.String(maxLength: 200),
                        DetailUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.SchoolId);
            
            CreateTable(
                "dbo.sns_PersonCertificate",
                c => new
                    {
                        CertificateNo = c.String(nullable: false, maxLength: 50),
                        PersonId = c.String(nullable: false, maxLength: 50),
                        CertificateName = c.String(maxLength: 50),
                        Level = c.String(maxLength: 50),
                        IssueOrgName = c.String(maxLength: 200),
                        IssueDate = c.DateTime(),
                        ExpiredDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CertificateNo)
                .ForeignKey("dbo.sns_Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.sns_PersonLink",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 50),
                        LinkPersonId = c.String(nullable: false, maxLength: 50),
                        LinkType = c.Int(nullable: false),
                        LinkRemark = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.PersonId, t.LinkPersonId })
                .ForeignKey("dbo.sns_Person", t => t.PersonId, cascadeDelete: false)
                .ForeignKey("dbo.sns_Person", t => t.LinkPersonId, cascadeDelete: false)
                .Index(t => t.PersonId)
                .Index(t => t.LinkPersonId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.sns_PersonLink", new[] { "LinkPersonId" });
            DropIndex("dbo.sns_PersonLink", new[] { "PersonId" });
            DropIndex("dbo.sns_PersonCertificate", new[] { "PersonId" });
            DropIndex("dbo.sns_StudyResume", new[] { "SchoolId" });
            DropIndex("dbo.sns_StudyResume", new[] { "PersonId" });
            DropIndex("dbo.sns_WorkResume", new[] { "OrgId" });
            DropIndex("dbo.sns_WorkResume", new[] { "PersonId" });
            DropIndex("dbo.sns_PersonContact", new[] { "ContactType" });
            DropIndex("dbo.sns_PersonContact", new[] { "PersonId" });
            DropForeignKey("dbo.sns_PersonLink", "LinkPersonId", "dbo.sns_Person");
            DropForeignKey("dbo.sns_PersonLink", "PersonId", "dbo.sns_Person");
            DropForeignKey("dbo.sns_PersonCertificate", "PersonId", "dbo.sns_Person");
            DropForeignKey("dbo.sns_StudyResume", "SchoolId", "dbo.sns_School");
            DropForeignKey("dbo.sns_StudyResume", "PersonId", "dbo.sns_Person");
            DropForeignKey("dbo.sns_WorkResume", "OrgId", "dbo.sns_Org");
            DropForeignKey("dbo.sns_WorkResume", "PersonId", "dbo.sns_Person");
            DropForeignKey("dbo.sns_PersonContact", "ContactType", "dbo.sns_Contact");
            DropForeignKey("dbo.sns_PersonContact", "PersonId", "dbo.sns_Person");
            DropTable("dbo.sns_PersonLink");
            DropTable("dbo.sns_PersonCertificate");
            DropTable("dbo.sns_School");
            DropTable("dbo.sns_StudyResume");
            DropTable("dbo.sns_WorkResume");
            DropTable("dbo.sns_PersonContact");
            DropTable("dbo.sns_Person");
            DropTable("dbo.sns_Org");
            DropTable("dbo.sns_Contact");
        }
    }
}
