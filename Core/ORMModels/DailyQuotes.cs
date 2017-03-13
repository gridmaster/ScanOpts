using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonModels
{
    public class DailyQuotes
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Exchange { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string instrumentType { get; set; }

        [Column(TypeName = "INT")]
        public int? timestamp { get; set; }

        public decimal? close { get; set; }
        public decimal? high { get; set; }
        public decimal? low { get; set; }
        public decimal? open { get; set; }
        public decimal? volume { get; set; }
        public decimal? unadjhigh { get; set; }
        public decimal? unadjlow { get; set; }
        public decimal? unadjclose { get; set; }
        public decimal? unadjopen { get; set; }
    }
}
