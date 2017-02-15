using Newtonsoft.Json;

namespace Core.Models
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "optionChain")]
        public OptionChain OptionChain { get; set; }
    }
}
