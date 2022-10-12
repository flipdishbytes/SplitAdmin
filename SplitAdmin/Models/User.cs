using Newtonsoft.Json;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    public class User : IOwner
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
#pragma warning restore CS8618
    }
}
