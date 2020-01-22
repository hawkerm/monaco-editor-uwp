using Newtonsoft.Json;

namespace Monaco.Languages
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.ilanguageextensionpoint.html
    /// </summary>
    public sealed class ILanguageExtensionPoint
    {
        [JsonProperty("aliases", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Aliases { get; set; }

        /// <summary>
        /// Uniform Resource Identifier (Uri) http://tools.ietf.org/html/rfc3986.
        /// This class is a simple parser which creates the basic component parts
        /// (http://tools.ietf.org/html/rfc3986#section-3) with minimal validation
        /// and encoding.
        ///
        /// foo://example.com:8042/over/there?name=ferret#nose
        /// \_/   \______________/\_________/ \_________/ \__/
        /// |           |            |            |        |
        /// scheme     authority       path        query   fragment
        /// |   _____________________|__
        /// / \ /                        \
        /// urn:example:animal:ferret:nose
        /// </summary>
        [JsonProperty("configuration", NullValueHandling = NullValueHandling.Ignore)]
        public IUri Configuration { get; set; }

        [JsonProperty("extensions", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Extensions { get; set; }

        [JsonProperty("filenamePatterns", NullValueHandling = NullValueHandling.Ignore)]
        public string[] FilenamePatterns { get; set; }

        [JsonProperty("filenames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Filenames { get; set; }

        [JsonProperty("firstLine", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstLine { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mimetypes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Mimetypes { get; set; }
    }
}
