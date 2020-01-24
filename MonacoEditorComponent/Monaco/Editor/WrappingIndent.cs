using Newtonsoft.Json;
using System;

namespace Monaco.Editor
{

    /// <summary>
    /// Control indentation of wrapped lines. Can be: 'none', 'same', 'indent' or 'deepIndent'.
    /// Defaults to 'same' in vscode and to 'none' in monaco-editor.
    /// </summary>
    [JsonConverter(typeof(WrappingIndentConverter))]
    public enum WrappingIndent { DeepIndent, Indent, None, Same };

    internal class WrappingIndentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(WrappingIndent) || t == typeof(WrappingIndent?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "deepIndent":
                    return WrappingIndent.DeepIndent;
                case "indent":
                    return WrappingIndent.Indent;
                case "none":
                    return WrappingIndent.None;
                case "same":
                    return WrappingIndent.Same;
            }
            throw new Exception("Cannot unmarshal type WrappingIndent");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (WrappingIndent)untypedValue;
            switch (value)
            {
                case WrappingIndent.DeepIndent:
                    serializer.Serialize(writer, "deepIndent");
                    return;
                case WrappingIndent.Indent:
                    serializer.Serialize(writer, "indent");
                    return;
                case WrappingIndent.None:
                    serializer.Serialize(writer, "none");
                    return;
                case WrappingIndent.Same:
                    serializer.Serialize(writer, "same");
                    return;
            }
            throw new Exception("Cannot marshal type WrappingIndent");
        }
    }
}
