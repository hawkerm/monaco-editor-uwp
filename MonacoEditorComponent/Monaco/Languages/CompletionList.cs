using Newtonsoft.Json;
using System.Collections.Generic;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.completionlist.html
    /// </summary>
    public sealed class CompletionList
    {
        [JsonProperty("incomplete", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsIncomplete { get; set; }

        [JsonProperty("suggestions")]
        public IList<CompletionItem> Items { get; set; } = new List<CompletionItem>();
    }
}
