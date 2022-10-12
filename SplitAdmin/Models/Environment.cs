using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class Environment
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("production")]
        public bool IsProduction { get; set; }
#pragma warning restore CS8618
    }
}
