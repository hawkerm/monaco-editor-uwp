using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// The initial editor dimension (to avoid measuring the container).
    /// </summary>
    public interface IDimension
    {
        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        double Height { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        double Width { get; set; }
    }
}
