///<reference path="../monaco-editor/monaco.d.ts" />
var registerCompletionItemProvider = function (languageId, characters) {
    return monaco.languages.registerCompletionItemProvider(languageId, {
        triggerCharacters: characters,
        provideCompletionItems: function (model, position, token, context) {
            return Parent.callEvent("CompletionItemProvider" + languageId, [JSON.stringify(position), JSON.stringify(context)]).then(function (result) {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
        //// TODO: support resolve method as well.
    });
};
//# sourceMappingURL=registerCompletionItemProvider.js.map