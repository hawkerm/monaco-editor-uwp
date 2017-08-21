using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco
{
    #pragma warning disable CS1591
    public interface IRange: IJsonable
    {
        [JsonProperty("startLineNumber")]
        uint StartLineNumber { get; }
        [JsonProperty("startColumn")]
        uint StartColumn { get; }
        [JsonProperty("endLineNumber")]
        uint EndLineNumber { get; }
        [JsonProperty("endColumn")]
        uint EndColumn { get; }
    }
    #pragma warning restore CS1591
}
