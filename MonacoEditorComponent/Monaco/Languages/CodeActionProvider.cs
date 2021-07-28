using Monaco;
using Monaco.Editor;
using Windows.Foundation;

namespace Monaco.Languages
{

    /// <summary>
    /// The code action interface defines the contract between extensions and
    /// the [light bulb](https://code.visualstudio.com/docs/editor/editingevolved#_code-action) feature.
    /// </summary>
    public interface CodeActionProvider
    {
        /// <summary>
        /// Provide commands for the given document and range.
        /// </summary>
        IAsyncOperation<CodeActionList> ProvideCodeActionsAsync(IModel model, Range range, CodeActionContext context);
    }
}

