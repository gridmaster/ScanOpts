using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class StandardDeviations
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }
        public int Count {get; set;}
        public double First { get; set; }
        public double Last { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(4)]
        public string Slope {get;set;}
        public double Difference { get; set; }
    }
}
