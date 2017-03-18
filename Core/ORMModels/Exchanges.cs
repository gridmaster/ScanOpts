using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class Exchanges
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Exchange { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        public string FullExchangeName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        public string ExchangeURL { get; set; }
        
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
    }
}
