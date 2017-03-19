using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.JsonModels;
using Core.JsonQptions;

namespace Core.ORMModels
{
    public class BaseCallPut
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }
        [Column(TypeName = "INT")]
        public int ExpirationDate { get; set; }
        public Strike strike { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
        public PercentChange PercentChange { get; set; }
        public OpenInterest openInterest { get; set; }
        public Change change { get; set; }
        public bool inTheMoney { get; set; }
        public ImpliedVolatility impliedVolatility { get; set; }
        public Volume volume { get; set; }
        public string contractSymbol { get; set; }
        public Ask Ask { get; set; }
        public LastTradeDate lastTradeDate { get; set; }
        public string contractSize { get; set; }
        public string currency { get; set; }
        public Expiration expiration { get; set; }
        public Bid bid { get; set; }
        public LastPrice lastPrice { get; set; }
    }

    #region Call Put Properties

    public class PercentChange : BaseRawFmt
    {
    }

    public class OpenInterest : BaseRawFmt
    {
    }

    public class Change : BaseRawFmt
    {
    }

    public class ImpliedVolatility : BaseRawFmt
    {
    }

    public class Volume : BaseRawFmtDate
    {
    }

    public class Ask : BaseRawFmt
    {
    }

    public class LastTradeDate : BaseRawFmtDate
    {
    }

    public class Expiration : BaseRawFmtDate
    {
    }

    public class Bid : BaseRawFmt
    {
    }

    public class LastPrice : BaseRawFmt
    {
    }
    #endregion Call Put Properties

}
