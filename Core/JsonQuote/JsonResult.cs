using Newtonsoft.Json;

namespace Core.JsonQuote
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "chart")]
        public Chart Chart { get; set; }
    }
}
