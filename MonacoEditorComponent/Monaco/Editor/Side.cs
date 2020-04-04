using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{

    /// <summary>
    /// Control the side of the minimap in editor.
    /// Defaults to 'right'.
    /// </summary>
    [JsonConverter(typeof(SideConverter))]
    public enum Side { Left, Right };

    internal class SideConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Side) || t == typeof(Side?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "left":
                    return Side.Left;
                case "right":
                    return Side.Right;
            }
            throw new Exception("Cannot unmarshal type Side");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Side)untypedValue;
            switch (value)
            {
                case Side.Left:
                    serializer.Serialize(writer, "left");
                    return;
                case Side.Right:
                    serializer.Serialize(writer, "right");
                    return;
            }
            throw new Exception("Cannot marshal type Side");
        }
    }
}
