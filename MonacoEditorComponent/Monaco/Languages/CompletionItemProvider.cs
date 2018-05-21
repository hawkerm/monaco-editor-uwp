using Monaco.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Monaco.Languages
{
    public interface CompletionItemProvider
    {
        [JsonProperty("triggerCharacters")]
        string[] TriggerCharacters { get; }

        IAsyncOperation<CompletionList> ProvideCompletionItemsAsync(IModel document, IPosition position, CompletionContext context);

        IAsyncOperation<CompletionItem> ResolveCompletionItemAsync(CompletionItem item);
    }
}
