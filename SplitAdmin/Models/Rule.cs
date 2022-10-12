using Newtonsoft.Json;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    public class Rule
    {
#pragma warning disable CS8618
        [JsonProperty("buckets")]
        public List<Bucket> Buckets { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }
#pragma warning restore CS8618
    }
}
