namespace ORMService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CallPuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuoteId = c.Int(nullable: false),
                        CallOrPut = c.String(maxLength: 4, unicode: false),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        ExpirationRaw = c.Int(nullable: false),
                        StrikeRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpirationFmt = c.String(maxLength: 60, unicode: false),
                        ExpirationLongFmt = c.String(maxLength: 60, unicode: false),
                        StrikeFmt = c.String(maxLength: 60, unicode: false),
                        StrikeLongFmt = c.String(maxLength: 60, unicode: false),
                        Date = c.DateTime(nullable: false),
                        PercentChangeRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentChangeFmt = c.String(maxLength: 60, unicode: false),
                        PercentChangeLongFmt = c.String(maxLength: 60, unicode: false),
                        OpenInterestRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenInterestFmt = c.String(maxLength: 60, unicode: false),
                        OpenInterestLongFmt = c.String(maxLength: 60, unicode: false),
                        ChangeRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChangeFmt = c.String(maxLength: 60, unicode: false),
                        ChangeLongFmt = c.String(maxLength: 60, unicode: false),
                        InTheMoney = c.Boolean(nullable: false),
                        ImpliedVolatilityRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImpliedVolatilityFmt = c.String(maxLength: 60, unicode: false),
                        ImpliedVolatilityLongFmt = c.String(maxLength: 60, unicode: false),
                        VolumeRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VolumeFmt = c.String(maxLength: 60, unicode: false),
                        VolumeLongFmt = c.String(maxLength: 60, unicode: false),
                        ContractSymbol = c.String(maxLength: 60, unicode: false),
                        AskRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AskFmt = c.String(maxLength: 60, unicode: false),
                        AskLongFmt = c.String(maxLength: 60, unicode: false),
                        LastTradeDateRaw = c.Int(nullable: false),
                        LastTradeDateFmt = c.String(maxLength: 60, unicode: false),
                        LastTradeDateLongFmt = c.String(maxLength: 60, unicode: false),
                        ContractSize = c.String(maxLength: 60, unicode: false),
                        Currency = c.String(maxLength: 20, unicode: false),
                        BidRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidFmt = c.String(maxLength: 60, unicode: false),
                        BidLongFmt = c.String(maxLength: 60, unicode: false),
                        LastPriceRaw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastPriceFmt = c.String(maxLength: 60, unicode: false),
                        LastPriceLongFmt = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DailyQuotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        Date = c.DateTime(nullable: false),
                        Exchange = c.String(maxLength: 20, unicode: false),
                        instrumentType = c.String(maxLength: 40, unicode: false),
                        timestamp = c.Int(),
                        close = c.Decimal(precision: 18, scale: 2),
                        high = c.Decimal(precision: 18, scale: 2),
                        low = c.Decimal(precision: 18, scale: 2),
                        open = c.Decimal(precision: 18, scale: 2),
                        volume = c.Decimal(precision: 18, scale: 2),
                        unadjhigh = c.Decimal(precision: 18, scale: 2),
                        unadjlow = c.Decimal(precision: 18, scale: 2),
                        unadjclose = c.Decimal(precision: 18, scale: 2),
                        unadjopen = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        Date = c.DateTime(nullable: false),
                        ExpirationDate = c.Long(nullable: false),
                        HasMiniOptions = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Straddles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        call_Id = c.Int(),
                        put_Id = c.Int(),
                        strike_ID = c.Int(),
                        Option_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calls", t => t.call_Id)
                .ForeignKey("dbo.Puts", t => t.put_Id)
                .ForeignKey("dbo.Strikes", t => t.strike_ID)
                .ForeignKey("dbo.Options", t => t.Option_Id)
                .Index(t => t.call_Id)
                .Index(t => t.put_Id)
                .Index(t => t.strike_ID)
                .Index(t => t.Option_Id);
            
            CreateTable(
                "dbo.Calls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuoteId = c.Int(nullable: false),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        ExpirationDate = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PercentChange_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentChange_fmt = c.String(maxLength: 60, unicode: false),
                        PercentChange_longFmt = c.String(maxLength: 60, unicode: false),
                        openInterest_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        openInterest_fmt = c.String(maxLength: 60, unicode: false),
                        openInterest_longFmt = c.String(maxLength: 60, unicode: false),
                        change_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        change_fmt = c.String(maxLength: 60, unicode: false),
                        change_longFmt = c.String(maxLength: 60, unicode: false),
                        inTheMoney = c.Boolean(nullable: false),
                        impliedVolatility_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        impliedVolatility_fmt = c.String(maxLength: 60, unicode: false),
                        impliedVolatility_longFmt = c.String(maxLength: 60, unicode: false),
                        volume_raw = c.Int(nullable: false),
                        volume_fmt = c.String(maxLength: 60, unicode: false),
                        volume_longFmt = c.String(maxLength: 60, unicode: false),
                        contractSymbol = c.String(),
                        Ask_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ask_fmt = c.String(maxLength: 60, unicode: false),
                        Ask_longFmt = c.String(maxLength: 60, unicode: false),
                        lastTradeDate_raw = c.Int(nullable: false),
                        lastTradeDate_fmt = c.String(maxLength: 60, unicode: false),
                        lastTradeDate_longFmt = c.String(maxLength: 60, unicode: false),
                        contractSize = c.String(),
                        currency = c.String(),
                        expiration_raw = c.Int(nullable: false),
                        expiration_fmt = c.String(maxLength: 60, unicode: false),
                        expiration_longFmt = c.String(maxLength: 60, unicode: false),
                        bid_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bid_fmt = c.String(maxLength: 60, unicode: false),
                        bid_longFmt = c.String(maxLength: 60, unicode: false),
                        lastPrice_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        lastPrice_fmt = c.String(maxLength: 60, unicode: false),
                        lastPrice_longFmt = c.String(maxLength: 60, unicode: false),
                        strike_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Strikes", t => t.strike_ID)
                .Index(t => t.strike_ID);
            
            CreateTable(
                "dbo.Strikes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QuoteID = c.Int(nullable: false),
                        raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fmt = c.String(maxLength: 60, unicode: false),
                        longFmt = c.String(maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Puts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuoteId = c.Int(nullable: false),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        ExpirationDate = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PercentChange_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentChange_fmt = c.String(maxLength: 60, unicode: false),
                        PercentChange_longFmt = c.String(maxLength: 60, unicode: false),
                        openInterest_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        openInterest_fmt = c.String(maxLength: 60, unicode: false),
                        openInterest_longFmt = c.String(maxLength: 60, unicode: false),
                        change_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        change_fmt = c.String(maxLength: 60, unicode: false),
                        change_longFmt = c.String(maxLength: 60, unicode: false),
                        inTheMoney = c.Boolean(nullable: false),
                        impliedVolatility_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        impliedVolatility_fmt = c.String(maxLength: 60, unicode: false),
                        impliedVolatility_longFmt = c.String(maxLength: 60, unicode: false),
                        volume_raw = c.Int(nullable: false),
                        volume_fmt = c.String(maxLength: 60, unicode: false),
                        volume_longFmt = c.String(maxLength: 60, unicode: false),
                        contractSymbol = c.String(),
                        Ask_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ask_fmt = c.String(maxLength: 60, unicode: false),
                        Ask_longFmt = c.String(maxLength: 60, unicode: false),
                        lastTradeDate_raw = c.Int(nullable: false),
                        lastTradeDate_fmt = c.String(maxLength: 60, unicode: false),
                        lastTradeDate_longFmt = c.String(maxLength: 60, unicode: false),
                        contractSize = c.String(),
                        currency = c.String(),
                        expiration_raw = c.Int(nullable: false),
                        expiration_fmt = c.String(maxLength: 60, unicode: false),
                        expiration_longFmt = c.String(maxLength: 60, unicode: false),
                        bid_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        bid_fmt = c.String(maxLength: 60, unicode: false),
                        bid_longFmt = c.String(maxLength: 60, unicode: false),
                        lastPrice_raw = c.Decimal(nullable: false, precision: 18, scale: 2),
                        lastPrice_fmt = c.String(maxLength: 60, unicode: false),
                        lastPrice_longFmt = c.String(maxLength: 60, unicode: false),
                        strike_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Strikes", t => t.strike_ID)
                .Index(t => t.strike_ID);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        Date = c.DateTime(nullable: false),
                        Exchange = c.String(maxLength: 20, unicode: false),
                        QuoteType = c.String(maxLength: 20, unicode: false),
                        QuoteSourceName = c.String(maxLength: 60, unicode: false),
                        Currency = c.String(maxLength: 20, unicode: false),
                        MarketState = c.String(maxLength: 40, unicode: false),
                        ShortName = c.String(maxLength: 400, unicode: false),
                        Market = c.String(maxLength: 60, unicode: false),
                        LongName = c.String(maxLength: 500, unicode: false),
                        PreMarketChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreMarketTime = c.Int(nullable: false),
                        PreMarketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreMarketChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PostMarketChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PostMarketTime = c.Int(nullable: false),
                        PostMarketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PostMarketChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeTimezoneName = c.String(maxLength: 60, unicode: false),
                        ExchangeTimezoneShortName = c.String(maxLength: 10, unicode: false),
                        GmtOffSetMilliseconds = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketPreviousClose = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ask = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidSize = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AskSize = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MessageBoardId = c.String(maxLength: 40, unicode: false),
                        FullExchangeName = c.String(maxLength: 60, unicode: false),
                        AverageDailyVolume3Month = c.Int(nullable: false),
                        AverageDailyVolume10Day = c.Int(nullable: false),
                        FiftyTwoWeekLowChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyTwoWeekLowChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyTwoWeekHighChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyTwoWeekHighChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyTwoWeekLow = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyTwoWeekHigh = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketTime = c.Int(nullable: false),
                        RegularMarketChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketOpen = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketDayHigh = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketDayLow = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegularMarketVolume = c.Int(nullable: false),
                        SharesOutstanding = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyDayAverage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyDayAverageChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FiftyDayAverageChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TwoHundredDayAverage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TwoHundredDayAverageChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TwoHundredDayAverageChangePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MarketCap = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SourceInterval = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpirationDates = c.String(maxLength: 1000, unicode: false),
                        Strikes = c.String(maxLength: 1000, unicode: false),
                        HasMiniOptions = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Symbols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(maxLength: 60, unicode: false),
                        CompanyName = c.String(maxLength: 400, unicode: false),
                        Exchange = c.String(maxLength: 20, unicode: false),
                        FullExchangeName = c.String(maxLength: 300, unicode: false),
                        Date = c.DateTime(nullable: false),
                        Select = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Straddles", "Option_Id", "dbo.Options");
            DropForeignKey("dbo.Straddles", "strike_ID", "dbo.Strikes");
            DropForeignKey("dbo.Straddles", "put_Id", "dbo.Puts");
            DropForeignKey("dbo.Puts", "strike_ID", "dbo.Strikes");
            DropForeignKey("dbo.Straddles", "call_Id", "dbo.Calls");
            DropForeignKey("dbo.Calls", "strike_ID", "dbo.Strikes");
            DropIndex("dbo.Puts", new[] { "strike_ID" });
            DropIndex("dbo.Calls", new[] { "strike_ID" });
            DropIndex("dbo.Straddles", new[] { "Option_Id" });
            DropIndex("dbo.Straddles", new[] { "strike_ID" });
            DropIndex("dbo.Straddles", new[] { "put_Id" });
            DropIndex("dbo.Straddles", new[] { "call_Id" });
            DropTable("dbo.Symbols");
            DropTable("dbo.Statistics");
            DropTable("dbo.Puts");
            DropTable("dbo.Strikes");
            DropTable("dbo.Calls");
            DropTable("dbo.Straddles");
            DropTable("dbo.Options");
            DropTable("dbo.DailyQuotes");
            DropTable("dbo.CallPuts");
        }
    }
}
