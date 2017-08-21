using Monaco.Helpers;
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
        public bool IsWholeLine { get; set; }
        public string[] HoverMessage { get; set; }
        public string[] GlyphMarginHoverMessage { get; set; }
        public CssLineStyle ClassName { get; set; }
        public CssGlyphStyle GlyphMarginClassName { get; set; }
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
}
