///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

var registerCodeLensProvider = function (languageId) {
    return monaco.languages.registerCodeLensProvider(languageId, {
        provideCodeLenses: function (model, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("ProvideCodeLenses" + languageId, promiseId,null,null));
            //return null;
            //var result = 
            //if (result) {
            //        return JSON.parse(result);
            //    }
            //    else {
            //        return null;
            //    }
        },
        resolveCodeLens: function (model, codeLens, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("ResolveCodeLens" + languageId, promiseId, stringifyForMarshalling(codeLens), null));
            //return null;
            //var result = 
            //if (result) {
            //        return JSON.parse(result);
            //    }
            //    else {
            //        return null;
            //    }
        }
        // TODO: onDidChange, don't know what this does.
    });
}