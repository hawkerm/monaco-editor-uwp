using Monaco.Editor;
using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public partial class CodeEditor
    {
        #region Reveal Methods
        public IAsyncAction RevealLineAsync(uint lineNumber)
        {
            return SendScriptAsync("editor.revealLine(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLineInCenterAsync(uint lineNumber)
        {
            return SendScriptAsync("editor.revealLineInCenter(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLineInCenterIfOutsideViewportAsync(uint lineNumber)
        {
            return SendScriptAsync("editor.revealLineInCenterIfOutsideViewport(" + lineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesAsync(uint startLineNumber, uint endLineNumber)
        {
            return SendScriptAsync("editor.revealLines(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesInCenterAsync(uint startLineNumber, uint endLineNumber)
        {
            return SendScriptAsync("editor.revealLinesInCenter(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
        }

        public IAsyncAction RevealLinesInCenterIfOutsideViewportAsync(uint startLineNumber, uint endLineNumber)
        {
            return SendScriptAsync("editor.revealLinesInCenterIfOutsideViewport(" + startLineNumber + ", " + endLineNumber + ")").AsAsyncAction();
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
            return SendScriptAsync("editor.revealPosition(JSON.parse('" + position.ToJson() + "'), " + JsonConvert.ToString(revealVerticalInCenter) + ", " + JsonConvert.ToString(revealHorizontal) + ")").AsAsyncAction();
        }

        public IAsyncAction RevealPositionInCenterAsync(IPosition position)
        {
            return SendScriptAsync("editor.revealPositionInCenter(JSON.parse('" + position.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealPositionInCenterIfOutsideViewportAsync(IPosition position)
        {
            return SendScriptAsync("editor.revealPositionInCenterIfOutsideViewport(JSON.parse('" + position.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeAsync(IRange range)
        {
            return SendScriptAsync("editor.revealRange(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeAtTopAsync(IRange range)
        {
            return SendScriptAsync("editor.revealRangeAtTop(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeInCenterAsync(IRange range)
        {
            return SendScriptAsync("editor.revealRangeInCenter(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }

        public IAsyncAction RevealRangeInCenterIfOutsideViewportAsync(IRange range)
        {
            return SendScriptAsync("editor.revealRangeInCenterIfOutsideViewport(JSON.parse('" + range.ToJson() + "'))").AsAsyncAction();
        }
        #endregion

        public IAsyncAction AddActionAsync(IActionDescriptor action)
        {
            var wref = new WeakReference<CodeEditor>(this);
            _parentAccessor.RegisterAction("Action" + action.Id, new Action(() => { if (wref.TryGetTarget(out CodeEditor editor)) { action?.Run(editor, null); } }));
            return InvokeScriptAsync("addAction", action).AsAsyncAction();
        }

        /// <summary>
        /// Invoke scripts, return value must be strings
        /// </summary>
        /// <param name="script">Script to invoke</param>
        /// <returns>An async operation result to string</returns>
        public IAsyncOperation<string> InvokeScriptAsync(string script)
        {
            return _view.InvokeScriptAsync("eval", new[] { script });
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler)
        {
            return AddCommandAsync(keybinding, handler, string.Empty);
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler, string context)
        {
            var name = "Command" + keybinding;
            _parentAccessor.RegisterAction(name, new Action(() => { handler?.Invoke(); }));
            return InvokeScriptAsync<string>("addCommand", new object[] { keybinding, name, context }).AsAsyncOperation();
        }

        public IAsyncOperation<ContextKey> CreateContextKeyAsync(string key, bool defaultValue)
        {
            var ck = new ContextKey(this, key, defaultValue);

            return InvokeScriptAsync("createContext", ck).ContinueWith((noop) =>
            {
                return ck;
            }).AsAsyncOperation();
        }

        public IModel GetModel()
        {
            return _model;
        }

        public IAsyncOperation<IEnumerable<Marker>> GetModelMarkersAsync() // TODO: Filter (string? owner, Uri? resource, int? take)
        {
            return SendScriptAsync<IEnumerable<Marker>>("monaco.editor.getModelMarkers();").AsAsyncOperation();
        }

        public IAsyncAction SetModelMarkersAsync(string owner, [ReadOnlyArray] IMarkerData[] markers)
        {
            return SendScriptAsync("monaco.editor.setModelMarkers(model, " + JsonConvert.ToString(owner) + ", " + JsonConvert.SerializeObject(markers) + ");").AsAsyncAction();
        }

        public IAsyncOperation<Position> GetPositionAsync()
        {
            return SendScriptAsync<Position>("editor.getPosition();").AsAsyncOperation();
        }

        public IAsyncAction SetPositionAsync(IPosition position)
        {
            return SendScriptAsync("editor.setPosition(" + JsonConvert.SerializeObject(position) + ");").AsAsyncAction();
        }

        /// <summary>
        /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.icommoncodeeditor.html#deltadecorations
        /// 
        /// Using <see cref="Decorations"/> Property to manipulate decorations instead of calling this directly.
        /// </summary>
        /// <param name="newDecorations"></param>
        /// <returns></returns>
        private IAsyncAction DeltaDecorationsHelperAsync([ReadOnlyArray] IModelDeltaDecoration[] newDecorations)
        {
            var newDecorationsAdjust = newDecorations ?? Array.Empty<IModelDeltaDecoration>();

            // Update Styles
            return InvokeScriptAsync("updateStyle", CssStyleBroker.GetInstance(this).GetStyles()).ContinueWith((noop) =>
            {
                // Send Command to Modify Decorations
                // IMPORTANT: Need to cast to object here as we want this to be a single array object passed as a parameter, not a list of parameters to expand.
                return InvokeScriptAsync("updateDecorations", (object)newDecorationsAdjust);
            }).AsAsyncAction();
        }
    }
}
