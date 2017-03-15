using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonModels
{
    public class Result
    {
        public string UnderlyingSymbol { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
        public List<decimal> ExpirationDates { get; set; }
        public List<decimal> Strikes { get; set; }
        public bool HasMiniOptions { get; set; }

        [JsonProperty(PropertyName = "quote")]
        public Statistics Statistics { get; set; }
        public Options Options { get; set; }
    }
}
