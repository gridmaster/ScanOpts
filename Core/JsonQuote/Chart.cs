using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.JsonModels
{
    public class Chart
    {
        [JsonProperty(PropertyName = "result")]
        public List<ORMModels.Result> Result { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
