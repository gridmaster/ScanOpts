namespace ORMService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DividendsAndSplits : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Calls", new[] { "strike_ID" });
            DropIndex("dbo.Puts", new[] { "strike_ID" });
            CreateIndex("dbo.Calls", "Strike_ID");
            CreateIndex("dbo.Puts", "Strike_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Puts", new[] { "Strike_ID" });
            DropIndex("dbo.Calls", new[] { "Strike_ID" });
            CreateIndex("dbo.Puts", "strike_ID");
            CreateIndex("dbo.Calls", "strike_ID");
        }
    }
}
