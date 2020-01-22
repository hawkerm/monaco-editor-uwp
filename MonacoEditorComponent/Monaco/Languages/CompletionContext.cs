using Newtonsoft.Json;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.completioncontext.html
    /// </summary>
    public sealed class CompletionContext
    {
        [JsonProperty("triggerCharacter", NullValueHandling = NullValueHandling.Ignore)]
        public string TriggerCharacter { get; set; }

        [JsonProperty("triggerKind")]
        public SuggestTriggerKind TriggerKind { get; set; }
    }
}
