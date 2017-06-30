using System;

namespace Core.ORMModels
{
    public class BollingerBand
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double SMA20 { get; set; }
        public double StandardDeviation { get; set; }
        public double UpperBand { get; set; }
        public double LowerBand { get; set; }
        public double BandRatio { get; set; }
    }
}
