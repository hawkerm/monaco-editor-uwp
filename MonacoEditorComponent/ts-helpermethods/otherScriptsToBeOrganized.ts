///<reference path="../monaco-editor/monaco.d.ts" />
declare var Parent: ParentAccessor;

declare var editor: monaco.editor.IStandaloneCodeEditor;
declare var model: monaco.editor.ITextModel;
declare var contexts: { [index: string]: monaco.editor.IContextKey<any> };//{};
declare var decorations: string[];
declare var modifingSelection:boolean; // Supress updates to selection when making edits.

var registerHoverProvider = function (languageId: string) {
    return monaco.languages.registerHoverProvider(languageId, {
        provideHover: function (model, position) {
            return Parent.callEvent("HoverProvider" + languageId, [JSON.stringify(position)]).then(result => {
                if (result) {
                    return JSON.parse(result);
                }
            });
        }
    });
}

var addAction = function (action: monaco.editor.IActionDescriptor) {
    action.run = function (ed) {
        Parent.callAction("Action" + action.id)
    };

    editor.addAction(action);
};

var addCommand = function (keybindingStr, handlerName, context) {
    return editor.addCommand(parseInt(keybindingStr), () => {
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
}

var updateContent = function (content) {
    // Need to ignore updates from us notifying of a change
    if (content != model.getValue()) {
        model.setValue(content);
    }
};
