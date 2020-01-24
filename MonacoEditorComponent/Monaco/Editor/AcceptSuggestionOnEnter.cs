using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{

    /// <summary>
    /// Accept suggestions on ENTER.
    /// Defaults to 'on'.
    /// </summary>
    [JsonConverter(typeof(AcceptSuggestionOnEnterConverter))]
    public enum AcceptSuggestionOnEnter { Off, On, Smart };

    internal class AcceptSuggestionOnEnterConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AcceptSuggestionOnEnter) || t == typeof(AcceptSuggestionOnEnter?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "off":
                    return AcceptSuggestionOnEnter.Off;
                case "on":
                    return AcceptSuggestionOnEnter.On;
                case "smart":
                    return AcceptSuggestionOnEnter.Smart;
            }
            throw new Exception("Cannot unmarshal type AcceptSuggestionOnEnter");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AcceptSuggestionOnEnter)untypedValue;
            switch (value)
            {
                case AcceptSuggestionOnEnter.Off:
                    serializer.Serialize(writer, "off");
                    return;
                case AcceptSuggestionOnEnter.On:
                    serializer.Serialize(writer, "on");
                    return;
                case AcceptSuggestionOnEnter.Smart:
                    serializer.Serialize(writer, "smart");
                    return;
            }
            throw new Exception("Cannot marshal type AcceptSuggestionOnEnter");
        }
    }
}
