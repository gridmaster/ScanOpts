using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class BollingerBands
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
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
        public double Volume { get; set; }

        [Column(TypeName = "INT")]
        public int? Timestamp { get; set; }
    }
}
