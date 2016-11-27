using Newtonsoft.Json;

namespace DDPAIDash.Core.Types
{
    public class Parameter<T>
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}