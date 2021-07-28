using Monaco;
using Newtonsoft.Json;

namespace Monaco.Languages
{
    public sealed class WorkspaceTextEdit
    {
        [JsonProperty("edit", NullValueHandling = NullValueHandling.Ignore)]
        public TextEdit Edit { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public WorkspaceEditMetadata Metadata { get; set; }

        [JsonProperty("modelVersionId", NullValueHandling = NullValueHandling.Ignore)]
        public double ModelVersionId { get; set; }

        [JsonProperty("resource", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Resource { get; set; }
    }
}
