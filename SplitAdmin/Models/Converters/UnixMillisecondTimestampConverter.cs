using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SplitAdmin.Models.Converters
{
    public class UnixMillisecondTimestampConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteRawValue(UnixTimestampFromDateTime((DateTime)value).ToString());
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return reader.Value == null ? _epoch : TimeFromUnixTimestamp((long)reader.Value);
        }

        private static DateTime TimeFromUnixTimestamp(long timestamp)
        {
            return new DateTime(_epoch.Ticks + (timestamp * TimeSpan.TicksPerMillisecond));
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            return (long)((date.Ticks - _epoch.Ticks) / TimeSpan.TicksPerMillisecond);
        }
    }
}
