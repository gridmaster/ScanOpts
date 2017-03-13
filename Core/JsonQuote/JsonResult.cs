using Newtonsoft.Json;

namespace Core.JsonModels.ORMModels
{
    public class JsonResult
    {
        [JsonProperty(PropertyName = "chart")]
        public Chart Chart { get; set; }
    }
}
