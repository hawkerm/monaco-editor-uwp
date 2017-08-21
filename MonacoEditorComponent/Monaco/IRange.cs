using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco
{
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
}
