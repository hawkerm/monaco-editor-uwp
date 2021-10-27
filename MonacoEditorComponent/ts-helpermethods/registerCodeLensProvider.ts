///<reference path="../monaco-editor/monaco.d.ts" />
declare var Accessor: ParentAccessor;

const registerCodeLensProvider = function (languageId) {
    return monaco.languages.registerCodeLensProvider(languageId, {
        provideCodeLenses: function (model, token) {
            return Accessor.callEvent("ProvideCodeLenses" + languageId, []).then(result => {
                if (result) {
                    const list: monaco.languages.CodeLensList = JSON.parse(result);

                    // Add dispose method for IDisposable that Monaco is looking for.
                    list.dispose = () => {};

                    return list;
                }
            });
        },
        resolveCodeLens: function (model, codeLens, token) {
            return Accessor.callEvent("ResolveCodeLens" + languageId, [JSON.stringify(codeLens)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
        // TODO: onDidChange, don't know what this does.
    });
}