using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/classes/monaco.uri.html
    /// </summary>
    public sealed class IUri
    {
        [JsonProperty("authority")]
        public string Authority { get; set; }

        [JsonProperty("fragment")]
        public string Fragment { get; set; }

        [JsonProperty("fsPath")]
        public string FSPath { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        public IUri() { }
    }
}
