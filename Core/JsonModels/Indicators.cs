using System.Collections.Generic;

namespace Core.JsonModels
{
    public class Indicators
    {
        public List<HistoryDetail.Quote> quote { get; set; }

        public List<UnAdjClose> unadjclose { get; set; }

        public List<UnAdjQuote> unadjquote { get; set; }
    }

    public class UnAdjClose
    {
        public List<decimal> unadjclose { get; set; }
    }

    public class UnAdjQuote
    {
        public List<decimal> unadjhigh { get; set; }
        public List<decimal> unadjlow { get; set; }
        public List<decimal> unadjclose { get; set; }
        public List<decimal> unadjopen { get; set; }
    }
}