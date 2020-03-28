using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditorfindoptions.html
    /// </summary>
    public sealed class IEditorFindOptions
    {
        [JsonProperty("autoFindInSelection")]
        public bool AutoFindInSelection { get; set; }
        [JsonProperty("seedSearchStringFromSelection")]
        public bool SeedSearchStringFromSelection { get; set; } //= true;
    }
}
