using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodeldeltadecoration.html
    /// </summary>
    public sealed class IModelDeltaDecoration
    {
        [JsonProperty("options")]
        public IModelDecorationOptions Options { get; private set; }

        [JsonProperty("range")]
        public IRange Range { get; private set; }

        public IModelDeltaDecoration(IRange range, IModelDecorationOptions options)
        {
            Range = range;
            Options = options;
        }
    }
}
