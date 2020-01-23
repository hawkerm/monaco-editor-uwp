using Newtonsoft.Json;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.completionlist.html
    /// </summary>
    public sealed class CompletionList
    {
        [JsonProperty("incomplete", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Incomplete { get; set; }

        [JsonProperty("suggestions")]
        public CompletionItem[] Suggestions { get; set; }
    }
}
