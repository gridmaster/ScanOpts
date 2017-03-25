using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ORMModels
{
    public class Symbols : IEnumerable<Symbols>
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string Symbol { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(400)]
        public string CompanyName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Exchange { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(300)]
        public string FullExchangeName { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        public bool Select { get; set; }

        #region implement IEnumerable<Symbols>

        List<Symbols> mylist = new List<Symbols>();

        public Symbols this[int index]
        {
            get { return mylist[index]; }
            set { mylist.Insert(index, value); }
        }

        public IEnumerator<Symbols> GetEnumerator()
        {
            return mylist.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion implement IEnumerable<Symbols>

        //[Column(TypeName = "INT")]
        //public int? timestamp { get; set; }

        //public decimal? close { get; set; }
        //public decimal? high { get; set; }
        //public decimal? low { get; set; }
        //public decimal? open { get; set; }
        //public decimal? volume { get; set; }
    }
}
