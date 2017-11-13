using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imarker.html
    /// </summary>
    public interface IMarker : IMarkerData
    {
        [JsonProperty("owner")]
        string Owner { get; }

        // TODO: Should I port over Monaco.Uri? https://microsoft.github.io/monaco-editor/api/classes/monaco.uri.html
        [JsonProperty("resource")]
        IUri Resource { get; }
    }
}
