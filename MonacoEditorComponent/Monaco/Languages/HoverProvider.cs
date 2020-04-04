using Monaco.Editor;
using Windows.Foundation;

namespace Monaco.Languages
{
    /// <summary>
    /// The hover provider interface defines the contract between extensions and
    /// the [hover](https://code.visualstudio.com/docs/editor/intellisense)-feature.
    /// </summary>
    public interface HoverProvider
    {
        /// <summary>
        /// Provide a hover for the given position and document. Multiple hovers at the same
        /// position will be merged by the editor. A hover can have a range which defaults
        /// to the word range at the position when omitted.
        /// </summary>
        IAsyncOperation<Hover> ProvideHover(IModel model, Position position);
    }
}
