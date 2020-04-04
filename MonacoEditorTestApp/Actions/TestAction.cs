using Monaco.Editor;
using System;
using Monaco;
using Windows.UI.Popups;
using System.Runtime.InteropServices.WindowsRuntime;

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

        public async void Run(CodeEditor editor, [ReadOnlyArray]object[] args)
        {
            var md = new MessageDialog("You have selected text:\n\n" + editor.SelectedText);
            await md.ShowAsync();

            editor.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
        }
    }
}
