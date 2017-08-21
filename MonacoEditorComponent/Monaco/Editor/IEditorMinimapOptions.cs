using System;
using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditorminimapoptions.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IEditorMinimapOptions : IJsonable
    {
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        [JsonProperty("maxColumn")]
        public int? MaxColumn { get; set; } // = 120
        [JsonProperty("renderCharacters")]
        public bool? RenderCharacters { get; set; } // = true
        [JsonProperty("showSlider")]
        public string ShowSlider { get; set; } // = "mouseover"; always

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    #pragma warning restore CS1591
}