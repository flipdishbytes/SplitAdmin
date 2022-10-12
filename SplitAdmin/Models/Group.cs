using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class Group : IOwner
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
#pragma warning restore CS8618
    }
}
