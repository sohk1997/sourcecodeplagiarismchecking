namespace SourceCodePlagiarismCheckingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Drink = c.String(nullable: false, maxLength: 500),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Data");
        }
    }
}
