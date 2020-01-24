using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// A structure defining a problem/warning/etc.
    /// </summary>
    interface IMarkerData : IRange
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        string Code { get; set; }

        [JsonProperty("endColumn")]
        uint EndColumn { get; set; }

        [JsonProperty("endLineNumber")]
        uint EndLineNumber { get; set; }

        [JsonProperty("message")]
        string Message { get; set; }

        [JsonProperty("relatedInformation", NullValueHandling = NullValueHandling.Ignore)]
        IRelatedInformation[] RelatedInformation { get; set; }

        [JsonProperty("severity")]
        int Severity { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        string Source { get; set; }

        [JsonProperty("startColumn")]
        uint StartColumn { get; set; }

        [JsonProperty("startLineNumber")]
        uint StartLineNumber { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        int[] Tags { get; set; }
    }
}
