using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class RolloutStatus
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
#pragma warning restore CS8618
    }
}
