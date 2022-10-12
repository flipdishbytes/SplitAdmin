using Newtonsoft.Json;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    internal class MultipleResultsResponse<T>
    {
#pragma warning disable CS8618
        [JsonProperty("objects")]
        public List<T> Objects { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
#pragma warning restore CS8618
    }
}
