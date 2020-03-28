using Newtonsoft.Json;
using System;
using System.Linq;

namespace Monaco.Helpers
{
    [JsonConverter(typeof(CssStyleConverter))]
    public interface ICssStyle
    {
        string Name { get; }

        string ToCss();
    }
    internal class CssStyleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(ICssStyle) || objectType.GetInterfaces().Contains(typeof(ICssStyle));

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => new NotSupportedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ICssStyle style)
            {
                writer.WriteValue(style.Name);
            }
        }
    }
}
