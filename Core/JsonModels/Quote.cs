using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonModels
{
    public class Quote
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Exchange { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string QuoteType { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string QuoteSourceName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Currency { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string MarketState { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(400)]
        public string ShortName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Market { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string LongName { get; set; }
        public decimal PreMarketChangePercent { get; set; }
        [Column(TypeName = "INT")]
        public int PreMarketTime { get; set; }
        public decimal PreMarketPrice { get; set; }
        public decimal PreMarketChange { get; set; }
        public decimal PostMarketChangePercent { get; set; }
        [Column(TypeName = "INT")]
        public int PostMarketTime { get; set; }
        public decimal PostMarketPrice { get; set; }
        public decimal PostMarketChange { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ExchangeTimezoneName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string ExchangeTimezoneShortName { get; set; }
        public decimal GmtOffSetMilliseconds { get; set; }
        public decimal RegularMarketChangePercent { get; set; }
        public decimal RegularMarketPreviousClose { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal BidSize { get; set; }
        public decimal AskSize { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string MessageBoardId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string FullExchangeName { get; set; }
        [Column(TypeName = "INT")]
        public int AverageDailyVolume3Month { get; set; }
        [Column(TypeName = "INT")]
        public int AverageDailyVolume10Day { get; set; }
        public decimal FiftyTwoWeekLowChange { get; set; }
        public decimal FiftyTwoWeekLowChangePercent { get; set; }
        public decimal FiftyTwoWeekHighChange { get; set; }
        public decimal FiftyTwoWeekHighChangePercent { get; set; }
        public decimal FiftyTwoWeekLow { get; set; }
        public decimal FiftyTwoWeekHigh { get; set; }
        public decimal RegularMarketPrice { get; set; }
        [Column(TypeName = "INT")]
        public int RegularMarketTime { get; set; }
        public decimal RegularMarketChange { get; set; }
        public decimal RegularMarketOpen { get; set; }
        public decimal RegularMarketDayHigh { get; set; }
        public decimal RegularMarketDayLow { get; set; }
        [Column(TypeName = "INT")]
        public int RegularMarketVolume { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal FiftyDayAverage { get; set; }
        public decimal FiftyDayAverageChange { get; set; }
        public decimal FiftyDayAverageChangePercent { get; set; }
        public decimal TwoHundredDayAverage { get; set; }
        public decimal TwoHundredDayAverageChange { get; set; }
        public decimal TwoHundredDayAverageChangePercent { get; set; }
        public decimal MarketCap { get; set; }
        public decimal SourceInterval { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string ExpirationDates { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string Strikes { get; set; }
        public bool HasMiniOptions { get; set; }
    }
}
