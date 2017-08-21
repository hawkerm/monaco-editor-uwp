using System;
using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditorfindoptions.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IEditorFindOptions : IJsonable
    {
        [JsonProperty("autoFindInSelection")]
        public bool AutoFindInSelection { get; set; }
        [JsonProperty("seedSearchStringFromSelection")]
        public bool SeedSearchStringFromSelection { get; set; } //= true;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    #pragma warning restore CS1591
}