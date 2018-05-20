using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodeldeltadecoration.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IModelDeltaDecoration
    {
        [JsonProperty("options")]
        public IModelDecorationOptions Options { get; private set; }

        [JsonProperty("range")]
        public IRange Range { get; private set; }

        public IModelDeltaDecoration(IRange range, IModelDecorationOptions options)
        {
            this.Range = range;
            this.Options = options;
        }
    }
    #pragma warning restore CS1591
}
