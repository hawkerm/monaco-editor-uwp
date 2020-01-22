using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{
    internal class HorizontalConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Horizontal) || t == typeof(Horizontal?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "auto":
                    return Horizontal.Auto;
                case "hidden":
                    return Horizontal.Hidden;
                case "visible":
                    return Horizontal.Visible;
            }
            throw new Exception("Cannot unmarshal type Horizontal");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Horizontal)untypedValue;
            switch (value)
            {
                case Horizontal.Auto:
                    serializer.Serialize(writer, "auto");
                    return;
                case Horizontal.Hidden:
                    serializer.Serialize(writer, "hidden");
                    return;
                case Horizontal.Visible:
                    serializer.Serialize(writer, "visible");
                    return;
            }
            throw new Exception("Cannot marshal type Horizontal");
        }

        public static readonly HorizontalConverter Singleton = new HorizontalConverter();
    }

}
