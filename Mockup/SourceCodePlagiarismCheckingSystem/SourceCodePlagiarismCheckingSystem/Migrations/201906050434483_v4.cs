namespace SourceCodePlagiarismCheckingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ISO3 = c.String(maxLength: 3),
                        CountryName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 250),
                        LastName = c.String(),
                        DayOfBirth = c.DateTime(nullable: false, storeType: "date"),
                        Gender = c.Int(nullable: false),
                        EmailAddress = c.String(maxLength: 500),
                        ProfilePicture = c.String(maxLength: 1000),
                        CountryId = c.Guid(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "CountryId", "dbo.Countries");
            DropIndex("dbo.Users", new[] { "CountryId" });
            DropTable("dbo.Users");
            DropTable("dbo.Countries");
        }
    }
}
