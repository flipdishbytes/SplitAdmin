using Newtonsoft.Json;
using SplitAdmin.Models.Converters;
using System;
using System.Collections.Generic;

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

        [JsonProperty("killed")]
        public bool? Killed { get; set; }

        [JsonProperty("treatments")]
        public List<Treatment>? Treatments { get; set; }

        [JsonProperty("defaultTreatment")]
        public string? DefaultTreatment { get; set; }

        [JsonProperty("baselineTreatment")]
        public string? BaselineTreatment { get; set; }

        [JsonProperty("trafficAllocation")]
        public int? TrafficAllocation { get; set; }

        [JsonProperty("rules")]
        public List<Rule>? Rules { get; set; }

        [JsonProperty("defaultRule")]
        public List<DefaultRule>? DefaultRule { get; set; }

        [JsonProperty("lastUpdateTime", ItemConverterType = typeof(UnixMillisecondTimestampConverter))]
        public DateTime? LastUpdateTime { get; set; }

        [JsonProperty("changeNumber")]
        public long? ChangeNumber { get; set; }

        [JsonProperty("lastTrafficReceivedAt", ItemConverterType = typeof(UnixMillisecondTimestampConverter))]
        public DateTime? LastTrafficReceivedAt { get; set; }
#pragma warning restore CS8618
    }

}
