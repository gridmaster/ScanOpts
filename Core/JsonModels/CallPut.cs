using System;

namespace Core.JsonModels
{
    public class CallPut
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public string CallOrPut { get; set; }
        public string Symbol { get; set; }
        public decimal ExpirationDate { get; set; }
        public decimal StrikeRaw { get; set; }
        public string StrikeFmt { get; set; }
        public string StrikeLongFmt { get; set; }
        public DateTime Date { get; set; }
        public decimal PercentChangeRaw { get; set; }
        public string PercentChangeFmt { get; set; }
        public string PercentChangeLongFmt { get; set; }
        public decimal OpenInterestRaw { get; set; }
        public string OpenInterestFmt { get; set; }
        public string OpenInterestLongFmt { get; set; }
        public decimal ChangeRaw { get; set; }
        public string ChangeFmt { get; set; }
        public string ChangeLongFmt { get; set; }
        public bool inTheMoney { get; set; }
        public decimal ImpliedVolatilityRaw { get; set; }
        public string ImpliedVolatilityFmt { get; set; }
        public string ImpliedVolatilityLongFmt { get; set; }
        public decimal VolumeRaw { get; set; }
        public string VolumeFmt { get; set; }
        public string VolumeLongFmt { get; set; }
        public string contractSymbol { get; set; }
        public decimal AskRaw { get; set; }
        public string AskFmt { get; set; }
        public string AskLongFmt { get; set; }
        public decimal LastTradeDateRaw { get; set; }
        public string LastTradeDateFmt { get; set; }
        public string LastTradeDateLongFmt { get; set; }
        public string contractSize { get; set; }
        public string currency { get; set; }
        public decimal ExpirationRaw { get; set; }
        public string ExpirationFmt { get; set; }
        public string ExpirationLongFmt { get; set; }
        public decimal BidRaw { get; set; }
        public string BidFmt { get; set; }
        public string BidLongFmt { get; set; }
        public LastPrice lastPrice { get; set; }
        public decimal LastPriceRaw { get; set; }
        public string LastPriceFmt { get; set; }
        public string LastPriceLongFmt { get; set; }
    }
}
