using System;
using Newtonsoft.Json;

namespace DDPAIDash.Core.Json.Converters
{
    internal class EnumConverter<T> : JsonConverter where T : struct
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            T result;

            Enum.TryParse<T>((string)reader.Value, out result);
            
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
