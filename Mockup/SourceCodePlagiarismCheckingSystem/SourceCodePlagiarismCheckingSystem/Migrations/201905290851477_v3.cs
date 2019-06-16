namespace SourceCodePlagiarismCheckingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SourceCode", "StartLine", c => c.Int());
            AddColumn("dbo.SourceCode", "EndLine", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SourceCode", "EndLine");
            DropColumn("dbo.SourceCode", "StartLine");
        }
    }
}
