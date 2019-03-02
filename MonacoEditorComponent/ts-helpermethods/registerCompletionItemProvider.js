///<reference path="../monaco-editor/monaco.d.ts" />
var registerCompletionItemProvider = function (languageId, characters) {
    return monaco.languages.registerCompletionItemProvider(languageId, {
        triggerCharacters: characters,
        provideCompletionItems: function (model, position, context, token) {
            return Parent.callEvent("CompletionItemProvider" + languageId, [JSON.stringify(position), JSON.stringify(context)]).then(function (result) {
                if (result) {
                    return JSON.parse(result);
                }
            });
        },
        resolveCompletionItem: function (model, position, item, token) {
            return Parent.callEvent("CompletionItemRequested" + languageId, [JSON.stringify(position), JSON.stringify(item)]).then(function (result) {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
    });
};
//# sourceMappingURL=registerCompletionItemProvider.js.map