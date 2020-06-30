///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

var registerColorProvider = function (languageId) {
    return monaco.languages.registerColorProvider(languageId, {
        provideColorPresentations: function (model, colorInfo, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("ProvideColorPresentations" + languageId, promiseId, stringifyForMarshalling(colorInfo), null));
            //var result = 
            //    if (result) {
            //        return (JSON.parse(result));
            //    }
            //    else {
            //        return (null);
            //    }
        },
        provideDocumentColors: function (model, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("ProvideDocumentColors" + languageId, promiseId, null,null));
            //var result = 
            //    if (result) {
            //        return (JSON.parse(result));
            //    }
            //    else {
            //        return (null);
            //    }
        }
    });
}