using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    /// <summary>
    /// New model decorations.
    /// </summary>
    public sealed class IModelDeltaDecoration
    {
        [JsonProperty("options")]
        public IModelDecorationOptions Options { get; private set; }

        [JsonProperty("range")]
        public IRange Range { get; private set; }

        public IModelDeltaDecoration(IRange range, IModelDecorationOptions options)
        {
            Range = range;
            Options = options;
        }
    }
}
