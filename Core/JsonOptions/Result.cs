using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Core.JsonQptions;
//using Core.ORMModels;

namespace Core.JsonOptions
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
        public Core.ORMModels.Statistics Statistics { get; set; }
        public Options Options { get; set; }
    }
}
