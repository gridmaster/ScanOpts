using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class CallPut
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(4)]
        public string CallOrPut { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }
        [Column(TypeName ="INT")]
        public int ExpirationRaw { get; set; }
        public decimal StrikeRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ExpirationFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ExpirationLongFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string StrikeFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string StrikeLongFmt { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
        public decimal PercentChangeRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string PercentChangeFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string PercentChangeLongFmt { get; set; }
        public decimal OpenInterestRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string OpenInterestFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string OpenInterestLongFmt { get; set; }
        public decimal ChangeRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ChangeFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ChangeLongFmt { get; set; }
        public bool inTheMoney { get; set; }
        public decimal ImpliedVolatilityRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ImpliedVolatilityFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string ImpliedVolatilityLongFmt { get; set; }
        public decimal VolumeRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string VolumeFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string VolumeLongFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string contractSymbol { get; set; }
        public decimal AskRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string AskFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string AskLongFmt { get; set; }
        public int LastTradeDateRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string LastTradeDateFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string LastTradeDateLongFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string contractSize { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string currency { get; set; }
        public decimal BidRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string BidFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string BidLongFmt { get; set; }
        public LastPrice lastPrice { get; set; }
        public decimal LastPriceRaw { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string LastPriceFmt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string LastPriceLongFmt { get; set; }
    }
}
