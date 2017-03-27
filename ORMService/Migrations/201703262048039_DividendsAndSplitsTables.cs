namespace ORMService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DividendsAndSplitsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dividends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DividendDate = c.Int(nullable: false),
                        DividendAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Symbol = c.String(),
                        Exchange = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Splits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SplitDate = c.Int(nullable: false),
                        Numerator = c.Int(nullable: false),
                        Denominator = c.Int(nullable: false),
                        Ratio = c.String(),
                        Date = c.DateTime(nullable: false),
                        Symbol = c.String(),
                        Exchange = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Splits");
            DropTable("dbo.Dividends");
        }
    }
}
