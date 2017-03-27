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
        public Strike Strike { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
        public PercentChange PercentChange { get; set; }
        public OpenInterest OpenInterest { get; set; }
        public Change Change { get; set; }
        public bool InTheMoney { get; set; }
        public ImpliedVolatility ImpliedVolatility { get; set; }
        public Volume Volume { get; set; }
        public string ContractSymbol { get; set; }
        public Ask Ask { get; set; }
        public LastTradeDate LastTradeDate { get; set; }
        public string ContractSize { get; set; }
        public string Currency { get; set; }
        public Expiration Expiration { get; set; }
        public Bid Bid { get; set; }
        public LastPrice LastPrice { get; set; }
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
