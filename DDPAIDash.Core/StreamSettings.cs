
using Newtonsoft.Json;

namespace DDPAIDash.Core
{
    internal class StreamSettings
    {
        public StreamSettings(int frameRate, int streamType)
        {
        }

        [JsonProperty("frmrate")]
        public int FrameRate { get; set; }
        [JsonProperty("stream_type")]
        public int StreamType { get; set; }
    }
}
