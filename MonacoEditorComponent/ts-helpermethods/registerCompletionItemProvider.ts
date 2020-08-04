///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

var registerCompletionItemProvider = (languageId, characters) =>
    monaco.languages.registerCompletionItemProvider(languageId,
        {
            triggerCharacters: characters,
            provideCompletionItems: (model, position, context, token) => callParentEventAsync(
                "CompletionItemProvider" + languageId,
                [JSON.stringify(position), JSON.stringify(context)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
                return null;
            }),
            resolveCompletionItem: (item, token) => callParentEventAsync("CompletionItemRequested" + languageId,
                [JSON.stringify(item)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
                return null;
            })
        });
