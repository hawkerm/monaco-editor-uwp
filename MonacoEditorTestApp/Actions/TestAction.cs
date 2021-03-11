using Monaco.Editor;
using System;
using Monaco;
using Microsoft.UI.Xaml.Controls;

namespace MonacoEditorTestApp.Actions
{
    class TestAction : IActionDescriptor
    {
        public string ContextMenuGroupId => "navigation";
        public float ContextMenuOrder => 1.5f;
        public string Id => "meta-test-action";
        public string KeybindingContext => null;
        public int[] Keybindings => new int[] { KeyMod.Chord(KeyMod.CtrlCmd | KeyCode.KEY_K, KeyMod.CtrlCmd | KeyCode.KEY_M) };
        public string Label => "Test Action";
        public string Precondition => null;

        public async void Run(CodeEditor editor, object[] args)
        {
            var md = new ContentDialog
            {
                Title = "Monaco Editor Test App",
                Content = "You have selected text:\n\n" + editor.SelectedText,
                CloseButtonText = "Ok"
            };
            md.XamlRoot = editor.XamlRoot;
            await md.ShowAsync();

            editor.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
        }
    }
}
