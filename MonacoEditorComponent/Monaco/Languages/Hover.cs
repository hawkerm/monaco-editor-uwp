using Newtonsoft.Json;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.hover.html
    /// </summary>
    public sealed class Hover
    {
        [JsonProperty("contents")]
        public IMarkdownString[] Contents { get; set; }

        [JsonProperty("range")]
        public IRange Range { get; set; }

        public Hover([ReadOnlyArray] string[] contents, IRange range) : this(contents, range, false) { }
        
        public Hover([ReadOnlyArray] string[] contents, IRange range, bool isTrusted)
        {
            Contents = contents.ToMarkdownString(isTrusted);
            Range = range;
        }
    }
}
