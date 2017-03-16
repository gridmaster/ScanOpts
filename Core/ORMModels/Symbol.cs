using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class Symbols
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(400)]
        public string CompanyName { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        //[Column(TypeName = "INT")]
        //public int? timestamp { get; set; }

        //public decimal? close { get; set; }
        //public decimal? high { get; set; }
        //public decimal? low { get; set; }
        //public decimal? open { get; set; }
        //public decimal? volume { get; set; }
    }
}
