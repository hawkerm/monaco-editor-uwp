using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imarker.html
    /// </summary>
    public sealed class Marker : IMarker
    {
        public string Owner { get; set; }

        public IUri Resource { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public Severity Severity { get; set; }

        public string Source { get; set; }

        public uint StartLineNumber { get; set; }

        public uint StartColumn { get; set; }

        public uint EndLineNumber { get; set; }

        public uint EndColumn { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
