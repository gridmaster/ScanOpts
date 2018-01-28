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

        [Column(TypeName = "INT")]
        public int? Timestamp { get; set; }
    }
}
