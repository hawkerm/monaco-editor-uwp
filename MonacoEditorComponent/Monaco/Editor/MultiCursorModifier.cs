using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{

    /// <summary>
    /// The modifier to be used to add multiple cursors with the mouse.
    /// Defaults to 'alt'
    /// </summary>
    [JsonConverter(typeof(MultiCursorModifierConverter))]
    public enum MultiCursorModifier { Alt, CtrlCmd };

    internal class MultiCursorModifierConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MultiCursorModifier) || t == typeof(MultiCursorModifier?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "alt":
                    return MultiCursorModifier.Alt;
                case "ctrlCmd":
                    return MultiCursorModifier.CtrlCmd;
            }
            throw new Exception("Cannot unmarshal type MultiCursorModifier");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MultiCursorModifier)untypedValue;
            switch (value)
            {
                case MultiCursorModifier.Alt:
                    serializer.Serialize(writer, "alt");
                    return;
                case MultiCursorModifier.CtrlCmd:
                    serializer.Serialize(writer, "ctrlCmd");
                    return;
            }
            throw new Exception("Cannot marshal type MultiCursorModifier");
        }
    }
}
