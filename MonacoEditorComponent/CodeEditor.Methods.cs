using Monaco.Editor;
using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;

namespace Monaco
{
    /// <summary>
    /// This file contains Monaco IEditor method implementations we can call on our control.
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditor.html
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.icommoncodeeditor.html
    /// </summary>
    public partial class CodeEditor
    {
        public IAsyncAction RevealPositionInCenterAsync(Position position)
        {
            return this.SendScriptAsync("editor.revealPositionInCenter(JSON.parse('" + position.ToJson() + "'))").AsAsyncAction();
        }

        /// <summary>
        /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.icommoncodeeditor.html#deltadecorations
        /// </summary>
        /// <param name="oldDecoractions"></param>
        /// <param name="newDecorations"></param>
        /// <returns></returns>
        public IAsyncOperation<IEnumerable<string>> DeltaDecorationsAsync([ReadOnlyArray] string[] oldDecoractions, [ReadOnlyArray] IModelDeltaDecoration[] newDecorations)
        {
            // Update Styles
            return InvokeScriptAsync("updateStyle", CssStyleBroker.Instance.GetStyles()).ContinueWith((noop) =>
            {
                // Send Command to Modify Decorations
                return SendScriptAsync(String.Format("JSON.stringify(editor.deltaDecorations({0}, {1}));",
                                                            Json.Parse(Json.StringArray(oldDecoractions)), Json.Parse(Json.ObjectArray(newDecorations.Cast<IJsonable>()))));
            }).ContinueWith((result) =>
            {
                // TODO: Figure out how to unwrap as I go?
                var jsondecorations = JsonArray.Parse(result.Result.Result);

                IList<string> decorations = new List<string>(jsondecorations.Count);
                for (int i = 0; i < jsondecorations.Count; i++)
                {
                    decorations.Add(jsondecorations[i].GetString());
                }

                return decorations.AsEnumerable<string>();
            }).AsAsyncOperation();
        }
    }
}
