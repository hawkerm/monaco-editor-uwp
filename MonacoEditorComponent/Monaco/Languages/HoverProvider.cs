using Monaco.Editor;
using Windows.Foundation;

namespace Monaco.Languages
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.hoverprovider.html
    /// Simplify things here with a delegate vs. forcing to use a custom class instance.
    /// </summary>
    public delegate IAsyncOperation<Hover> HoverProvider(IModel model, IPosition position);
}
