///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

const registerCodeActionProvider = function (languageId) {
    return monaco.languages.registerCodeActionProvider(languageId, {
        provideCodeActions: function (model, range, context, token) {
            return Parent.callEvent("ProvideCodeActions" + languageId, [JSON.stringify(range), JSON.stringify(context)]).then(result => {
                if (result) {
                    const list:monaco.languages.CodeActionList = JSON.parse(result);

                    // TODO: Do this in other helpers where this method is expected to minimize JavaScript exceptions
                    // Add dispose method for IDisposable that Monaco is looking for.
                    list.dispose = () => {};

                    return list;
                }
            });
        },
    });
}