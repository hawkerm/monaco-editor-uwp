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
    public sealed class IModelDecorationOptions
    {
        [JsonProperty("isWholeLine")]
        public bool IsWholeLine { get; set; }

        [JsonProperty("hoverMessage")]
        public IMarkdownString[] HoverMessage { get; set; }

        [JsonProperty("glyphMarginHoverMessage")]
        public IMarkdownString[] GlyphMarginHoverMessage { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("className")]
        public CssLineStyle ClassName { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("glyphMarginClassName")]
        public CssGlyphStyle GlyphMarginClassName { get; set; }

        [JsonConverter(typeof(CssStyleConverter))]
        [JsonProperty("inlineClassName")]
        public CssInlineStyle InlineClassName { get; set; }

        // TODO: Provide LinesDecorationsClassName

        [JsonProperty("inlineClassNameAffectsLetterSpacing")]
        public bool InlineClassNameAffectsLetterSpacing { get; set; }

        [JsonProperty("stickiness")]
        public TrackedRangeStickiness Stickiness { get; set; }

        [JsonProperty("zIndex")]
        public int ZIndex { get; set; }
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
