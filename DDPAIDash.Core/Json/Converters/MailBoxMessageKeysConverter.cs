using System;
using Newtonsoft.Json;
using DDPAIDash.Core.Types;

namespace DDPAIDash.Core.Json.Converters
{
    internal class MailBoxMessageKeysConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            MailBoxMessageKeys result;

            if(!Enum.TryParse<MailBoxMessageKeys>((string) reader.Value, out result))
            {
                result = MailBoxMessageKeys.Unknown;
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
