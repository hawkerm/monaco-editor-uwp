using Monaco.Editor;
using Monaco.Helpers;
using Newtonsoft.Json;
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
    /// Action delegate for <see cref="CodeEditor.AddCommandAsync(int, CommandHandler)"/> and <see cref="CodeEditor.AddCommandAsync(int, CommandHandler, string)"/>.
    /// </summary>
    public delegate void CommandHandler();

    /// <summary>
    /// This file contains Monaco IEditor method implementations we can call on our control.
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditor.html
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.icommoncodeeditor.html
    /// </summary>
    #pragma warning disable CS1591
    public partial class CodeEditor
    {
        #region Reveal Methods
        public IAsyncAction RevealLineAsync(uint lineNumber)
        {
            return this.SendScriptAsync("editor.revealLine(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLineInCenterAsync(uint lineNumber)
        {
            return this.SendScriptAsync("editor.revealLineInCenter(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLineInCenterIfOutsideViewportAsync(uint lineNumber)
        {
            return this.SendScriptAsync("editor.revealLineInCenterIfOutsideViewport(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesAsync(uint startLineNumber, uint endLineNumber)
        {
            return this.SendScriptAsync("editor.revealLines(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesInCenterAsync(uint startLineNumber, uint endLineNumber)
        {
            return this.SendScriptAsync("editor.revealLinesInCenter(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesInCenterIfOutsideViewportAsync(uint startLineNumber, uint endLineNumber)
        {
            return this.SendScriptAsync("editor.revealLinesInCenterIfOutsideViewport(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealPositionAsync(IPosition position)
        {
            return RevealPositionAsync(position, false, false);
        }

        public IAsyncAction RevealPositionAsync(IPosition position, bool revealVerticalInCenter)
        {
            return RevealPositionAsync(position, revealVerticalInCenter, false);
        }

        public IAsyncAction RevealPositionAsync(IPosition position, bool revealVerticalInCenter, bool revealHorizontal)
        {
            return this.SendScriptAsync("editor.revealPosition(JSON.parse('" + position.ToJson() + "'), " + JsonConvert.ToString(revealVerticalInCenter) + ", " + JsonConvert.ToString(revealHorizontal) + ")").AsAsyncAction();
        }

        public IAsyncAction RevealPositionInCenterAsync(IPosition position)
        {
            return this.SendScriptAsync("editor.revealPositionInCenter(JSON.parse('" + position.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealPositionInCenterIfOutsideViewportAsync(IPosition position)
        {
            return this.SendScriptAsync("editor.revealPositionInCenterIfOutsideViewport(JSON.parse('" + position.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeAsync(IRange range)
        {
            return this.SendScriptAsync("editor.revealRange(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeAtTopAsync(IRange range)
        {
            return this.SendScriptAsync("editor.revealRangeAtTop(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeInCenterAsync(IRange range)
        {
            return this.SendScriptAsync("editor.revealRangeInCenter(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeInCenterIfOutsideViewportAsync(IRange range)
        {
            return this.SendScriptAsync("editor.revealRangeInCenterIfOutsideViewport(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }
        #endregion

        public IAsyncAction AddActionAsync(IActionDescriptor action)
        {
            _parentAccessor.RegisterAction("Action" + action.Id, new Action(() => { action?.Run(this); }));
            return this.InvokeScriptAsync("addAction", JsonConvert.SerializeObject(action)).AsAsyncAction();
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler)
        {
            return this.AddCommandAsync(keybinding, handler, string.Empty);
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler, string context)
        {
            var name = "Command" + keybinding;
            _parentAccessor.RegisterAction(name, new Action(() => { handler?.Invoke(); }));
            return this.InvokeScriptAsync("addCommand", keybinding.ToString(), name, context).AsAsyncOperation();
        }

        public IAsyncOperation<ContextKey> CreateContextKeyAsync(string key, bool defaultValue)
        {
            var ck = new ContextKey(this, key, defaultValue);

            return this.InvokeScriptAsync("createContext", JsonConvert.SerializeObject(ck)).ContinueWith((noop) =>
            {
                return ck;
            }).AsAsyncOperation();
        }

        public IModel GetModel()
        {
            return this._model;
        }

        public IAsyncOperation<Position> GetPositionAsync()
        {
            return this.SendScriptAsync("JSON.stringify(editor.getPosition());").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Position>(value);
                }

                return null;
            }).AsAsyncOperation();
        }

        /// <summary>
        /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.icommoncodeeditor.html#deltadecorations
        /// 
        /// Recommand using ThreadSafe Property Decorations to manipulate decorations instead of calling this directly.
        /// </summary>
        /// <param name="oldDecoractions"></param>
        /// <param name="newDecorations"></param>
        /// <returns></returns>
        private IAsyncOperation<IEnumerable<string>> DeltaDecorationsAsync([ReadOnlyArray] string[] oldDecoractions, [ReadOnlyArray] IModelDeltaDecoration[] newDecorations)
        {
            var newDecorationsAdjust = newDecorations ?? new IModelDeltaDecoration[0];

            // Update Styles
            return InvokeScriptAsync("updateStyle", CssStyleBroker.Instance.GetStyles()).ContinueWith((noop) =>
            {
                // Send Command to Modify Decorations
                return SendScriptAsync(String.Format("JSON.stringify(editor.deltaDecorations({0}, {1}));",
                                                            Json.Parse(Json.StringArray(oldDecoractions)), Json.Parse(Json.ObjectArray(newDecorationsAdjust.Cast<IJsonable>()))));
            }).ContinueWith((result) =>
            {
                // TODO: Figure out how to unwrap as I go?
                var ret = result?.Result?.Result;
                if (string.IsNullOrWhiteSpace(ret)) // Guard against Issue #5?
                {
                    ret = "[]";
                }

                var jsondecorations = JsonArray.Parse(ret);

                IList<string> decorations = new List<string>(jsondecorations.Count);
                for (int i = 0; i < jsondecorations.Count; i++)
                {
                    decorations.Add(jsondecorations[i].GetString());
                }

                return decorations.AsEnumerable<string>();
            }).AsAsyncOperation(); // TODO: Do Array Cast Here once making this private/protected.
        }
    }
    #pragma warning restore CS1591
}
