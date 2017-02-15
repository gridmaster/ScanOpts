using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Result
    {
        public string UnderlyingSymbol { get; set; }
        public DateTime Date { get; set; }
        public List<decimal> ExpirationDates { get; set; }
        public List<decimal> Strikes { get; set; }
        public bool HasMiniOptions { get; set; }
        public Quote Quote { get; set; }
        public Options Options { get; set; }
    }
}
