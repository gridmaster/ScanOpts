namespace ORMService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Symbols", "Selected", c => c.Boolean(nullable: false));
            DropColumn("dbo.Symbols", "Select");
            //DropColumn("dbo.Symbols", "Capacity");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Symbols", "Capacity", c => c.Int(nullable: false));
            AddColumn("dbo.Symbols", "Select", c => c.Boolean(nullable: false));
            DropColumn("dbo.Symbols", "Selected");
        }
    }
}
