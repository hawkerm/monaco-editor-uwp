using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.iwordatposition.html
    /// </summary>
    public sealed class WordAtPosition : IWordAtPosition
    {
        /// <summary>
        /// Column where the word ends.
        /// </summary>
        public uint EndColumn { get; private set; }

        /// <summary>
        /// Column where the word starts.
        /// </summary>
        public uint StartColumn { get; private set; }

        /// <summary>
        /// The word.
        /// </summary>
        public string Word { get; private set; }
    }
}
