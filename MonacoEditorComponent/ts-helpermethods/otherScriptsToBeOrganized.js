///<reference path="../monaco-editor/monaco.d.ts" />
var registerHoverProvider = function (languageId) {
    return monaco.languages.registerHoverProvider(languageId, {
        provideHover: function (model, position) {
            return Parent.callEvent("HoverProvider" + languageId, [JSON.stringify(position)]).then(function (result) {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
    });
};
var addAction = function (action) {
    action.run = function (ed) {
        Parent.callAction("Action" + action.id);
    };
    editor.addAction(action);
};
var addCommand = function (keybindingStr, handlerName, context) {
    return editor.addCommand(parseInt(keybindingStr), function () {
        Parent.callAction(handlerName);
    }, context);
};
var createContext = function (context) {
    if (context) {
        contexts[context.key] = editor.createContextKey(context.key, context.defaultValue);
    }
};
var updateContext = function (key, value) {
    contexts[key].set(value);
};
var updateContent = function (content) {
    // Need to ignore updates from us notifying of a change
    if (content != model.getValue()) {
        model.setValue(content);
    }
};
//# sourceMappingURL=otherScriptsToBeOrganized.js.map