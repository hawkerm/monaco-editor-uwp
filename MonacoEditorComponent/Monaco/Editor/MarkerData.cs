using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imarkerdata.html
    /// </summary>
    public sealed class MarkerData : IMarkerData
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("severity")]
        public Severity Severity { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("startLineNumber")]
        public uint StartLineNumber { get; set; }

        [JsonProperty("startColumn")]
        public uint StartColumn { get; set; }

        [JsonProperty("endLineNumber")]
        public uint EndLineNumber { get; set; }

        [JsonProperty("endColumn")]
        public uint EndColumn { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
