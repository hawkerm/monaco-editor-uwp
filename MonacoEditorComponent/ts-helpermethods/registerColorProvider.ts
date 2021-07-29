///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

const registerColorProvider = function (languageId) {
    return monaco.languages.registerColorProvider(languageId, {
        provideColorPresentations: function (model, colorInfo, token) {
            return Parent.callEvent("ProvideColorPresentations" + languageId, [JSON.stringify(colorInfo)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
            });
        },
        provideDocumentColors: function (model, token) {
            return Parent.callEvent("ProvideDocumentColors" + languageId, []).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
    });
}