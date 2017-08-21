using System;
using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditorscrollbaroptions.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IEditorScrollbarOptions : IJsonable
    {
        [JsonProperty("arrowSize")]
        public int? ArrowSize { get; set; } // = 11;
        [JsonProperty("handleMouseWheel")]
        public bool? HandleMouseWheel { get; set; } // = true;
        [JsonProperty("horizontal")]
        public string Horizontal { get; set; } // = "auto"; visible, hidden
        [JsonProperty("horizontalHasArrows")]
        public bool? HorizontalHasArrows { get; set; }
        [JsonProperty("horizontalScrollbarSize")]
        public uint? HorizontalScrollbarSize { get; set; } // = 10; (px)
        [JsonProperty("horizontalSliderSize")]
        public uint? HorizontalSliderSize { get; set; } // = 10; (px)
        [JsonProperty("useShadows")]
        public bool? UseShadows { get; set; }
        [JsonProperty("vertical")]
        public string Vertical { get; set; } // = "auto"; visible, hidden
        [JsonProperty("verticalHasArrows")]
        public bool? VerticalHasArrows { get; set; }
        [JsonProperty("verticalScrollbarSize")]
        public uint? VerticalScrollbarSize { get; set; } // = 10; (px)
        [JsonProperty("verticalSliderSize")]
        public uint? VerticalSliderSize { get; set; } // = 10; (px)

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    #pragma warning restore CS1591
}