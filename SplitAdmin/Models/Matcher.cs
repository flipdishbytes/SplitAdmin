using Newtonsoft.Json;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    public class Matcher
    {
#pragma warning disable CS8618
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("strings")]
        public List<string> Strings { get; set; }

        [JsonProperty("negate")]
        public bool? Negate { get; set; }

        [JsonProperty("string")]
        public string String { get; set; }
#pragma warning restore CS8618
    }
}
