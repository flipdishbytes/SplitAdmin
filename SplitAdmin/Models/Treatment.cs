using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class Treatment
    {
#pragma warning disable CS8618
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("configurations")]
        public string Configurations { get; set; }
#pragma warning restore CS8618
    }
}
