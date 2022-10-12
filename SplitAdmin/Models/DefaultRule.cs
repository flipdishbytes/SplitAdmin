using Newtonsoft.Json;

namespace SplitAdmin.Models
{
    public class DefaultRule
    {
#pragma warning disable CS8618
        [JsonProperty("treatment")]
        public string Treatment { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
#pragma warning restore CS8618
    }
}
