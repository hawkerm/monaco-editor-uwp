using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.completionlist.html
    /// </summary>
    public sealed class CompletionList : IEnumerable<CompletionItem> // TODO: Could I somehow also just make this a list?  Investigate Json Serialization Converter helper for that?
    {
        [JsonProperty("isIncomplete", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsIncomplete { get; set; }

        [JsonProperty("items")]
        public IList<CompletionItem> Items { get; set; } = new List<CompletionItem>();

        public IEnumerator<CompletionItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
