using System.Collections.Generic;

namespace Core.JsonModels
{
    public class Meta
    {
        public string currency { get; set; }
        public string symbol { get; set; }
        public string exchangeName { get; set; }
        public string instrumentType { get; set; }
        public int firstTradeDate { get; set; }
        public int gmtoffset { get; set; }
        public string timezone { get; set; }
        public CurrentTradingPeriod currentTradingPeriod { get; set; }
        public string dataGranularity { get; set; }
        public List<string> validRanges { get; set; }
    }
}