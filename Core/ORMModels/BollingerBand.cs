using System;

namespace Core.ORMModels
{
    public class BollingerBand
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal SMA20 { get; set; }
        public decimal UpperBand { get; set; }
        public decimal LowerBand { get; set; }
        public decimal BandRatio { get; set; }
    }
}
