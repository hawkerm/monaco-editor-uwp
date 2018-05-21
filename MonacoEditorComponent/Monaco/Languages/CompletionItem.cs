using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.completionitem.html
    /// </summary>
    public sealed class CompletionItem
    {
        ////public ISingleEditOperation[] AdditionalTextEdits { get; set; }

        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public Command Command { get; set; }

        [JsonProperty("commitCharacters", NullValueHandling = NullValueHandling.Ignore)]
        public string[] CommitCharacters { get; set; }

        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }

        [JsonProperty("documentation", NullValueHandling = NullValueHandling.Ignore)]
        public IMarkdownString Documentation { get; set; }

        [JsonProperty("filterText", NullValueHandling = NullValueHandling.Ignore)]
        public string FilterText { get; set; }

        [JsonProperty("insertText", NullValueHandling = NullValueHandling.Ignore)]
        public SnippetString InsertText { get; set; }

        [JsonProperty("kind")]
        public CompletionItemKind Kind { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public IRange Range { get; set; }

        [JsonProperty("sortText", NullValueHandling = NullValueHandling.Ignore)]
        public string SortText { get; set; }

        public CompletionItem(string label, CompletionItemKind kind)
        {
            Label = label;
            Kind = kind;
        }
    }
}
