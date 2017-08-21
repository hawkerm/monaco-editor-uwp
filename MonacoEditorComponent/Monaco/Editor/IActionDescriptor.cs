using System;
using Monaco.Helpers;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.iactiondescriptor.html
    /// https://microsoft.github.io/monaco-editor/playground.html#interacting-with-the-editor-adding-an-action-to-an-editor-instance
    /// </summary>
    #pragma warning disable CS1591
    public interface IActionDescriptor
    {
        /**
         * https://github.com/Microsoft/vscode/blob/master/src/vs/monaco.d.ts#L1907
		 * Control if the action should show up in the context menu and where.
		 * The context menu of the editor has these default:
		 *   navigation - The navigation group comes first in all cases.
		 *   1_modification - This group comes next and contains commands that modify your code.
		 *   9_cutcopypaste - The last default group with the basic editing commands.
		 * You can also create your own group.
		 * Defaults to null (don't show in context menu).
		 */
        [JsonProperty("contextMenuGroupId", NullValueHandling = NullValueHandling.Ignore)]
        string ContextMenuGroupId { get; }

        [JsonProperty("contextMenuOrder", NullValueHandling = NullValueHandling.Ignore)]
        float ContextMenuOrder { get; }
        
        [JsonProperty("id")]
        string Id { get; }

        /// <summary>
        /// <see cref="IContextKey"/>
        /// </summary>
        [JsonProperty("keybindingContext", NullValueHandling = NullValueHandling.Ignore)]
        string KeybindingContext { get; }

        /// <summary>
        /// <see cref="Monaco.KeyMod"/>, <see cref="Monaco.KeyCode"/>, and <see cref="Monaco.KeyMod.Chord(int, int)"/>
        /// </summary>
        [JsonProperty("keybindings")]
        int[] Keybindings { get; }

        [JsonProperty("label")]
        string Label { get; }

        /// <summary>
        /// <see cref="IContextKey"/>
        /// </summary>
        [JsonProperty("precondition", NullValueHandling = NullValueHandling.Ignore)]
        string Precondition { get; }

        void Run(CodeEditor editor);
    }
    #pragma warning restore CS1591
}