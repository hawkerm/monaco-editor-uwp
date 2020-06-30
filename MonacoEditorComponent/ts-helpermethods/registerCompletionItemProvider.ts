///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

var registerCompletionItemProvider = function (languageId, characters) {
    return monaco.languages.registerCompletionItemProvider(languageId, {
        triggerCharacters: characters,
        provideCompletionItems: function (model, position, context, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("CompletionItemProvider" + languageId, promiseId, stringifyForMarshalling(position), stringifyForMarshalling(context)));
            //var result = 
            //    if (result) {
            //        return (JSON.parse(result));
            //    }
            //    else {
            //        return (null);
            //    }

        },
        resolveCompletionItem: function (model, position, item, token) {
            return invokeAsyncMethod((promiseId) => Parent.callEvent("CompletionItemRequested" + languageId, promiseId, stringifyForMarshalling(position), stringifyForMarshalling(item)));
            //return null;
            //var result = 
            //if (result) {
            //        return (JSON.parse(result));
            //    }
            //    else {
            //        return (null);
            //    }

        }
    });
}