using Newtonsoft.Json;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    public class Condition
    {
#pragma warning disable CS8618
        [JsonProperty("combiner")]
        public string Combiner { get; set; }

        [JsonProperty("matchers")]
        public List<Matcher> Matchers { get; set; }
#pragma warning restore CS8618
    }
}
