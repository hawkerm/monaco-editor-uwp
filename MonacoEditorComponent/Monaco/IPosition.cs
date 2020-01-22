using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco
{
    public interface IPosition : IJsonable
    {
        [JsonProperty("column")]
        uint Column { get; }
        [JsonProperty("lineNumber")]
        uint LineNumber { get; }
    }
}
