using Monaco;
using Monaco.Editor;

namespace Monaco.Languages
{
    public sealed class CodeActionList // IDisposable??
    {
        [Newtonsoft.Json.JsonProperty("actions", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CodeAction[] Actions { get; set; }

    }
}

