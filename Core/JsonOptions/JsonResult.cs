using Newtonsoft.Json;

namespace Core.JsonOptions
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "optionChain")]
        public OptionChain OptionChain { get; set; }
    }
}
