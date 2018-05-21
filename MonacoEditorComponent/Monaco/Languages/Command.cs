using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.command.html
    /// </summary>
    public sealed class Command
    {
        //// TODO: Find a usage example for this in Monaco.
        [JsonProperty("arguments", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Arguments { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tooltip", NullValueHandling = NullValueHandling.Ignore)]
        public string Tooltip { get; set; }
    }
}
