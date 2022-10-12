using Newtonsoft.Json;
using SplitAdmin.Models.Converters;


namespace SplitAdmin.Models
{
    public class Split
    {
#pragma warning disable CS8618
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("trafficType")]
        public TrafficType TrafficType { get; set; }

        [JsonProperty("creationTime", ItemConverterType = typeof(UnixMillisecondTimestampConverter))]
        public DateTime CreationTime { get; set; }

        [JsonProperty("rolloutStatus")]
        public RolloutStatus RolloutStatus { get; set; }

        [JsonProperty("rolloutStatusTimestamp", ItemConverterType = typeof(UnixMillisecondTimestampConverter))]
        public DateTime RolloutStatusTimestamp { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("owners")]
        public List<Owner> Owners { get; set; }
#pragma warning restore CS8618
    }

}
