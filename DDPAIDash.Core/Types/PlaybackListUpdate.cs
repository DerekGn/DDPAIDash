using System;
using Newtonsoft.Json;
using DDPAIDash.Core.Json.Converters;

namespace DDPAIDash.Core.Types
{
    public class PlaybackListUpdate
    {
        [JsonProperty("action")]
        [JsonConverter(typeof(EnumConverter<PlaybackAction>))]
        public PlaybackAction Action  { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("starttime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? StartTime { get; set; }

        [JsonProperty("endtime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? EndTime { get; set; }
    }
}
