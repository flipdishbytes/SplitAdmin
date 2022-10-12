using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class Tag
    {
#pragma warning disable CS8618
        [JsonProperty("name")]
        public string Name { get; set; }
#pragma warning restore CS8618
    }
}
