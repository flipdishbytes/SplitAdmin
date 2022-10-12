using Newtonsoft.Json;
using SplitAdmin.Models.Converters;
using System;
using System.Collections.Generic;

namespace SplitAdmin.Models
{
    public class Segment
    {
#pragma warning disable CS8618
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("trafficType")]
        public TrafficType TrafficType { get; set; }

        [JsonProperty("creationTime", ItemConverterType = typeof(UnixMillisecondTimestampConverter))]
        public DateTime CreationTime { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
#pragma warning restore CS8618
    }

}
