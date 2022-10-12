using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class Owner
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
#pragma warning restore CS8618
    }
}
