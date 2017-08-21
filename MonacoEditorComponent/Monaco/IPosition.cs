using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco
{
    #pragma warning disable CS1591
    public interface IPosition: IJsonable
    {
        [JsonProperty("column")]
        uint Column { get; }
        [JsonProperty("lineNumber")]
        uint LineNumber { get; }
    }
    #pragma warning restore CS1591
}
