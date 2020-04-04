using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{

    /// <summary>
    /// Enable highlighting of matching brackets.
    /// Defaults to 'always'.
    /// </summary>
    [JsonConverter(typeof(MatchBracketsConverter))]
    public enum MatchBrackets { Always, Near, Never };

    internal class MatchBracketsConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MatchBrackets) || t == typeof(MatchBrackets?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "always":
                    return MatchBrackets.Always;
                case "near":
                    return MatchBrackets.Near;
                case "never":
                    return MatchBrackets.Never;
            }
            throw new Exception("Cannot unmarshal type MatchBrackets");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MatchBrackets)untypedValue;
            switch (value)
            {
                case MatchBrackets.Always:
                    serializer.Serialize(writer, "always");
                    return;
                case MatchBrackets.Near:
                    serializer.Serialize(writer, "near");
                    return;
                case MatchBrackets.Never:
                    serializer.Serialize(writer, "never");
                    return;
            }
            throw new Exception("Cannot marshal type MatchBrackets");
        }
    }
}
