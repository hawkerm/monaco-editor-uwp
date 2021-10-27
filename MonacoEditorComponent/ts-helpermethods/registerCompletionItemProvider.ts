///<reference path="../monaco-editor/monaco.d.ts" />
declare var Accessor: ParentAccessor;

const registerCompletionItemProvider = function (languageId, characters) {
    return monaco.languages.registerCompletionItemProvider(languageId, {
        triggerCharacters: characters,
        provideCompletionItems: function (model, position, context, token) {
            return Accessor.callEvent("CompletionItemProvider" + languageId, [JSON.stringify(position), JSON.stringify(context)]).then(result => {
                if (result) {
                    const list: monaco.languages.CompletionList = JSON.parse(result);

                    // Add dispose method for IDisposable that Monaco is looking for.
                    list.dispose = () => { };

                    return list;
                }
            });
        },
        resolveCompletionItem: function (item, token) {
            return Accessor.callEvent("CompletionItemRequested" + languageId, [JSON.stringify(item)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
    });
}