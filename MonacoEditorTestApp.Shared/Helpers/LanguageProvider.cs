using Monaco;
using Monaco.Editor;
using Monaco.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MonacoEditorTestApp.Helpers
{
    public class LanguageProvider : CompletionItemProvider
    {
        public string[] TriggerCharacters => new string[] { "c" };

        public IAsyncOperation<CompletionList> ProvideCompletionItemsAsync(IModel model, Position position, CompletionContext context)
        {
            throw new NotImplementedException();
        }
        public IAsyncOperation<CompletionList> ProvideCompletionItemsAsync(IModel document, IPosition position, CompletionContext context)
        {
            return AsyncInfo.Run(async delegate (CancellationToken cancelationToken)
            {
                var textUntilPosition = await document.GetValueInRangeAsync(new Range(1, 1, position.LineNumber, position.Column));

                if (textUntilPosition.EndsWith("boo"))
                {
                    return new CompletionList()
                    {
                        Suggestions =new []
                        {
                            new CompletionItem("booyah","booyah", CompletionItemKind.Folder),
                            new CompletionItem("booboo","booboo", CompletionItemKind.File),
                        }
                    };
                }
                else if (context.TriggerKind == CompletionTriggerKind.TriggerCharacter)
                {
                    return new CompletionList()
                    {
                        Suggestions = new[]
                        {
                            new CompletionItem("class","class", CompletionItemKind.Keyword),
                            new CompletionItem("cookie","cookie", CompletionItemKind.Reference),
                        }
                    };
                }

                return new CompletionList()
                {
                    Suggestions = new[]
                        {
                        new CompletionItem("foreach","foreach", CompletionItemKind.Snippet)
                        {
                            // https://code.visualstudio.com/docs/editor/userdefinedsnippets#_snippet-syntax
                            InsertText = "foreach (var ${2:element} in ${1:array}) {\n\t$0\n}"
                        }
                    }
                };
            });
        }


        public IAsyncOperation<CompletionItem> ResolveCompletionItemAsync(IModel model, Position position, CompletionItem item)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<CompletionItem> ResolveCompletionItemAsync(CompletionItem item)
        {
            throw new NotImplementedException();
        }
    }
}
