using Monaco.Editor;
using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            return this.InvokeScriptAsync("addAction", action).AsAsyncAction();
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler)
        {
            return this.AddCommandAsync(keybinding, handler, string.Empty);
        }

        public IAsyncOperation<string> AddCommandAsync(int keybinding, CommandHandler handler, string context)
        {
            var name = "Command" + keybinding;
            _parentAccessor.RegisterAction(name, new Action(() => { handler?.Invoke(); }));
            return this.InvokeScriptAsync("addCommand", new object[] { keybinding, name, context }).AsAsyncOperation();
        }

        public IAsyncOperation<ContextKey> CreateContextKeyAsync(string key, bool defaultValue)
        {
            var ck = new ContextKey(this, key, defaultValue);

            return this.InvokeScriptAsync("createContext", ck).ContinueWith((noop) =>
            {
                return ck;
            }).AsAsyncOperation();
        }

        public IModel GetModel()
        {
            return this._model;
        }

        public IAsyncOperation<IEnumerable<Marker>> GetModelMarkersAsync() // TODO: Filter (string? owner, Uri? resource, int? take)
        {
            return this.SendScriptAsync("JSON.stringify(monaco.editor.getModelMarkers());").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Marker[]>(value).AsEnumerable();
                }

                return Array.Empty<Marker>();
            }).AsAsyncOperation();
        }

        public IAsyncAction SetModelMarkersAsync(string owner, [ReadOnlyArray] IMarkerData[] markers)
        {
            return this.SendScriptAsync("monaco.editor.setModelMarkers(model, " + JsonConvert.ToString(owner) + ", " + JsonConvert.SerializeObject(markers) + ");").AsAsyncAction();
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
        /// Using <see cref="Decorations"/> Property to manipulate decorations instead of calling this directly.
        /// </summary>
        /// <param name="newDecorations"></param>
        /// <returns></returns>
        private IAsyncAction DeltaDecorationsHelperAsync([ReadOnlyArray] IModelDeltaDecoration[] newDecorations)
        {
            var newDecorationsAdjust = newDecorations ?? Array.Empty<IModelDeltaDecoration>();

            // Update Styles
            return InvokeScriptAsync("updateStyle", CssStyleBroker.Instance.GetStyles()).ContinueWith((noop) =>
            {
                // Send Command to Modify Decorations
                // IMPORTANT: Need to cast to object here as we want this to be a single array object passed as a parameter, not a list of parameters to expand.
                return InvokeScriptAsync("updateDecorations", (object)newDecorationsAdjust);
            }).AsAsyncAction();
        }
    }
    #pragma warning restore CS1591
}
