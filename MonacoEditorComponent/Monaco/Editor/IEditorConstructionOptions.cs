using Newtonsoft.Json;

namespace Monaco.Editor
{
    public interface IEditorConstructionOptions : IEditorOptions
    {
        [JsonProperty("dimension", NullValueHandling = NullValueHandling.Ignore)]
        IDimension Dimension { get; set; }
    }
}
