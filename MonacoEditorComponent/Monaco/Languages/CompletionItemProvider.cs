using Monaco.Editor;
using Newtonsoft.Json;
using Windows.Foundation;

namespace Monaco.Languages
{
    public interface CompletionItemProvider
    {
        [JsonProperty("triggerCharacters", NullValueHandling = NullValueHandling.Ignore)]
        string[] TriggerCharacters { get; }

        IAsyncOperation<CompletionList> ProvideCompletionItemsAsync(IModel document, IPosition position, CompletionContext context);

        IAsyncOperation<CompletionItem> ResolveCompletionItemAsync(CompletionItem item);
    }
}
