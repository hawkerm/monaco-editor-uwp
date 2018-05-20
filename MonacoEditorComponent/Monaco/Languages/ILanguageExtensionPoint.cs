using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Monaco.Languages
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.ilanguageextensionpoint.html
    /// </summary>
    public sealed class ILanguageExtensionPoint
    {
        #pragma warning disable CS1591
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("aliases")]
        public string[] Aliases { get; set; }
        [JsonProperty("configuration")]
        public string Configuration { get; set; }
        [JsonProperty("extensions")]
        public string[] Extensions { get; set; }
        [JsonProperty("filenames")]
        public string[] Filenames { get; set; }
        [JsonProperty("filenamePatterns")]
        public string[] FilenamePatterns { get; set; }
        [JsonProperty("firstLine")]
        public string FirstLine { get; set; }        
        [JsonProperty("mimetypes")]
        public string[] Mimetypes { get; set; }        
        #pragma warning restore CS1591
    }
}
