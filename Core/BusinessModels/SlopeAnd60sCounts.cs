using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.BusinessModels
{
    public class SlopeAnd60sCounts
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }

        [Column(TypeName = "INT")]
        public int SymbolId { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Exchange { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string InstrumentType { get; set; }

        [Column(TypeName = "INT")]
        public int? Timestamp { get; set; }

        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Open { get; set; }
        public int? Volume { get; set; }
        public decimal? SMA60High { get; set; }
        public decimal? SMA60Low { get; set; }
        public decimal? SMA60Close { get; set; }
        public int? SMA60Volume { get; set; }
        public decimal? Slope60High { get; set; }
        public decimal? Slope60Low { get; set; }
        public decimal? Slope60Close { get; set; }
        public int? Slope60Volume { get; set; }
        public decimal? Ratio60High { get; set; }
        public decimal? Ratio60Low { get; set; }
        public decimal? Ratio60Close { get; set; }
    }
}