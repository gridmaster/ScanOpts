using System;

namespace Core.JsonModels
{
    public class Quote
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal ExpirationDate { get; set; }
        public string Exchange { get; set; }
        public string QuoteType { get; set; }
		public string QuoteSourceName { get; set; }
        public string Currency { get; set; }
        public string MarketState { get; set; }
        public string ShortName { get; set; }
        public string Market { get; set; }
        public string LongName { get; set; }
        public decimal PreMarketChangePercent { get; set; }
        public decimal PreMarketTime { get; set; }
        public decimal PreMarketPrice { get; set; }
        public decimal PreMarketChange { get; set; }
        public decimal PostMarketChangePercent { get; set; }
        public decimal PostMarketTime { get; set; }
        public decimal PostMarketPrice { get; set; }
        public decimal PostMarketChange { get; set; }
        public string ExchangeTimezoneName { get; set; }
        public string ExchangeTimezoneShortName { get; set; }
        public decimal GmtOffSetMilliseconds { get; set; }
        public decimal RegularMarketChangePercent { get; set; }
        public decimal RegularMarketPreviousClose { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal BidSize { get; set; }
        public decimal AskSize { get; set; }
        public string MessageBoardId { get; set; }
        public string FullExchangeName { get; set; }
        public decimal AverageDailyVolume3Month { get; set; }
        public decimal AverageDailyVolume10Day { get; set; }
        public decimal FiftyTwoWeekLowChange { get; set; }
        public decimal FiftyTwoWeekLowChangePercent { get; set; }
        public decimal FiftyTwoWeekHighChange { get; set; }
        public decimal FiftyTwoWeekHighChangePercent { get; set; }
        public decimal FiftyTwoWeekLow { get; set; }
        public decimal FiftyTwoWeekHigh { get; set; }
        public decimal RegularMarketPrice { get; set; }
        public decimal RegularMarketTime { get; set; }
        public decimal RegularMarketChange { get; set; }
        public decimal RegularMarketOpen { get; set; }
        public decimal RegularMarketDayHigh { get; set; }
        public decimal RegularMarketDayLow { get; set; }
        public decimal RegularMarketVolume { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal FiftyDayAverage { get; set; }
        public decimal FiftyDayAverageChange { get; set; }
        public decimal FiftyDayAverageChangePercent { get; set; }
        public decimal TwoHundredDayAverage { get; set; }
        public decimal TwoHundredDayAverageChange { get; set; }
        public decimal TwoHundredDayAverageChangePercent { get; set; }
        public decimal MarketCap { get; set; }
        public decimal SourceInterval { get; set; }
        public string ExpirationDates { get; set; }
        public string Strikes { get; set; }
        public bool HasMiniOptions { get; set; }
    }
}
