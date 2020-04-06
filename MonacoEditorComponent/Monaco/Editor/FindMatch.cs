using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    public sealed class FindMatch
    {
        [JsonProperty("matches", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string[] Matches { get; set; }

        [JsonProperty("range", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [JsonConverter(typeof(InterfaceToClassConverter<IRange, Range>))]
        public IRange Range { get; set; }
    }
}

