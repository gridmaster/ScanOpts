using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.JsonModels
{
    public class Option
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        
        [Column(TypeName = "DATETIME")]
        public DateTime Date { get; set; }
        public long ExpirationDate { get; set; }
        public bool HasMiniOptions { get; set; }

        [JsonProperty(PropertyName = "straddles")]
        public List<Straddles> Straddles { get; set; }
    }
}
