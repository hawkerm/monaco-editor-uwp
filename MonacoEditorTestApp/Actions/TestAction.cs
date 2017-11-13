using Monaco.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monaco;
using Windows.UI.Popups;

namespace MonacoEditorTestApp.Actions
{
    class TestAction : IActionDescriptor
    {
        public string ContextMenuGroupId => "navigation";
        public float ContextMenuOrder => 1.5f;
        public string Id => "meta-test-action";
        public string KeybindingContext => null;
        public int[] Keybindings => new int[] { Monaco.KeyMod.Chord(Monaco.KeyMod.CtrlCmd | Monaco.KeyCode.KEY_K, Monaco.KeyMod.CtrlCmd | Monaco.KeyCode.KEY_M) };
        public string Label => "Test Action";
        public string Precondition => null;

        public async void Run(CodeEditor editor)
        {
            var md = new MessageDialog("You have selected text:\n\n" + editor.SelectedText);
            await md.ShowAsync();

            editor.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
    }
}
