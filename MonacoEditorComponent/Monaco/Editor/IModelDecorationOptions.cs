using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodeldecorationoptions.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IModelDecorationOptions: IJsonable
    {
        [JsonProperty("isWholeLine")]
        public bool IsWholeLine { get; set; }

        [JsonProperty("hoverMessage")]
        public string[] HoverMessage { get; set; }

        [JsonProperty("glyphMarginHoverMessage")]
        public string[] GlyphMarginHoverMessage { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("className")]
        public CssLineStyle ClassName { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("glyphMarginClassName")]
        public CssGlyphStyle GlyphMarginClassName { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("inlineClassName")]
        public CssInlineStyle InlineClassName { get; set; }

        // TODO: Use JsonConvert
        public string ToJson()
        {
            StringBuilder output = new StringBuilder("{", 100);
            output.Append("\"isWholeLine\": ");
            output.Append(IsWholeLine.ToString().ToLower());
            output.Append(",");
            if (ClassName != null)
            {
                output.Append("\"className\": \"");
                output.Append(ClassName.Name);
                output.Append("\",");
            }
            if (GlyphMarginClassName != null)
            {
                output.Append("\"glyphMarginClassName\": \"");
                output.Append(GlyphMarginClassName.Name);
                output.Append("\",");
            }
            if (InlineClassName != null)
            {
                output.Append("\"inlineClassName\": \"");
                output.Append(InlineClassName.Name);
                output.Append("\",");
            }
            if (HoverMessage != null && HoverMessage.Length > 0)
            {
                output.Append(String.Format("\"hoverMessage\": {0},", Json.StringArray(HoverMessage)));
            }
            if (GlyphMarginHoverMessage != null && GlyphMarginHoverMessage.Length > 0)
            {
                output.Append(String.Format("\"glyphMarginHoverMessage\": {0},", Json.StringArray(GlyphMarginHoverMessage)));
            }
            output.Remove(output.Length - 1, 1); // Remove Trailing Comma
            output.Append("}");

            return output.ToString();
        }
    }
    #pragma warning restore CS1591

    internal class CssStyleConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var style = value as ICssStyle;
            writer.WriteValue(style.Name);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ICssStyle);
        }
    }
}
