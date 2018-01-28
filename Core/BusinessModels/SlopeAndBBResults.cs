using System;

namespace Core.BusinessModels
{
    public class SlopeAndBBResults
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal SMA20 { get; set; }
        public decimal SMA50 { get; set; }
        public decimal SMA200 { get; set; }
        public decimal StandardDeviation { get; set; }
        public decimal UpperBand { get; set; }
        public decimal LowerBand { get; set; }
        public decimal BandRatio { get; set; }
        public decimal Volume { get; set; }

        public decimal SlopeClose { get; set; }
        public decimal Slope20 { get; set; }
        public decimal Slope50 { get; set; }
        public decimal Slope200 { get; set; }
        public decimal SlopeStandardDeviation { get; set; }
        public decimal SlopeUpperBand { get; set; }
        public decimal SlopeLowerBand { get; set; }
        public decimal SlopeBandRatio { get; set; }

        public decimal RatioClose { get; set; }
        public decimal Ratio20 { get; set; }
        public decimal Ratio50 { get; set; }
        public decimal Ratio200 { get; set; }
        public decimal RatioStandardDeviation { get; set; }
        public decimal RatioUpperBand { get; set; }
        public decimal RatioLowerBand { get; set; }
        public decimal RatioBandRatio { get; set; }
    }
}
