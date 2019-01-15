using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditorconstructionoptions.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class IEditorConstructionOptions : IEditorOptions, IJsonable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("acceptSuggestionOnCommitCharacter")]
        public bool? AcceptSuggestionOnCommitCharacter { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("acceptSuggestionOnEnter")]
        public string AcceptSuggestionOnEnter { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("accessibilitySupport")]
        public string AccessibilitySupport { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("ariaLabel")]
        public string AriaLabel { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("autoClosingBrackets")]
        public bool? AutoClosingBrackets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("codeLens")]
        public bool? CodeLens { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("contextMenu")]
        public bool? ContextMenu { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("cursorBlinking")]
        public string CursorBlinking { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("cursorStyle")]
        public string CursorStyle { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("disableLayerHinting")]
        public bool? DisableLayerHinting { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("disableMonospaceOptimizations")]
        public bool? DisableMonospaceOptimizations { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("dragAndDrop")]
        public bool? DragAndDrop { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("emptySelectionClipboard")]
        public bool? EmptySelectionClipboard { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("find")]
        public IEditorFindOptions Find { get { return GetPropertyValue<IEditorFindOptions>(); } set { SetPropertyValue(value); } }
        [JsonProperty("fixedOverflowWidgets")]
        public bool? FixedOverflowWidgets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("folding")]
        public bool? Folding { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("fontFamily")]
        public string FontFamily { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("fontLigatures")]
        public bool? FontLigatures { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("fontSize")]
        public uint? FontSize { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("fontWeight")]
        public string FontWeight { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("formatOnPaste")]
        public bool? FormatOnPaste { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("glyphMargin")]
        public bool? GlyphMargin { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("hideCursorInOverviewRuler")]
        public bool? HideCursorInOverviewRuler { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("hover")]
        public bool? Hover { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("iconsInSuggestions")]
        public bool? IconsInSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("letterSpacing")]
        public int? LetterSpacing { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("lineDecorationsWidth")]
        public int? LineDecorationsWidth { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("lineHeight")]
        public int? LineHeight { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("lineNumbers")]
        public string LineNumbers { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("lineNumbersMinChars")]
        public int? LineNumbersMinChars { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("links")]
        public bool? Links { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("matchBrackets")]
        public bool? MatchBrackets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("minimap")]
        public IEditorMinimapOptions Minimap { get { return GetPropertyValue<IEditorMinimapOptions>(); } set { SetPropertyValue(value); } }
        [JsonProperty("mouseWheelScrollSensitivity")]
        public int? MouseWheelScrollSensitivity { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("mouseWheelZoom")]
        public bool? MouseWheelZoom { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("multiCursorModifier")]
        public string MultiCursorModifier { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("occurrencesHighlight")]
        public bool? OccurrencesHighlight { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("overviewRulerBorder")]
        public bool? OverviewRulerBorder { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("overviewRulerLanes")]
        public uint? OverviewRulerLanes { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("parameterHints")]
        public bool? ParameterHints { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("quickSuggestions")]
        public bool? QuickSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("quickSuggestionsDelay")]
        public uint? QuickSuggestionsDelay { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("readOnly")]
        public bool? ReadOnly { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("renderControlCharacters")]
        public bool? RenderControlCharacters { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("renderIndentGuides")]
        public bool? RenderIndentGuides { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("renderLineHighlight")]
        public string RenderLineHighlight { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("renderWhitespace")]
        public string RenderWhitespace { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("revealHorizontalRightPadding")]
        public uint? RevealHorizontalRightPadding { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("roundedSelection")]
        public bool? RoundedSelection { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("rulers")]
        public uint[] Rulers { get { return GetPropertyValue<uint[]>(); } set { SetPropertyValue(value); } }
        [JsonProperty("scrollBeyondLastLine")]
        public bool? ScrollBeyondLastLine { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("scrollbar")]
        public IEditorScrollbarOptions Scrollbar { get { return GetPropertyValue<IEditorScrollbarOptions>(); } set { SetPropertyValue(value); } }
        [JsonProperty("selectOnLineNumbers")]
        public bool? SelectOnLineNumbers { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("selectionClipboard")]
        public bool? SelectionClipboard { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("selectionHighlight")]
        public bool? SelectionHighlight { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("showFoldingControls")]
        public string ShowFoldingControls { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("snipperSuggestions")]
        public string SnippetSuggestions { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("stopRenderingLineAfter")]
        public int? StopRenderingLineAfter { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("suggestFontSize")]
        public uint? SuggestFontSize { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("suggestLineHeight")]
        public int? SuggestLineHeight { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("suggestOnTriggerCharacters")]
        public bool? SuggestOnTriggerCharacters { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("useTabStops")]
        public bool? UseTabStops { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordBasedSuggestions")]
        public bool? WordBasedSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordSeparators")]
        public string WordSeparators { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrap")]
        public string WordWrap { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrapBreakAfterCharacters")]
        public string WordWrapBreakAfterCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrapBeforeCharacters")]
        public string WordWrapBreakBeforeCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrapObtrusiveCharacters")]
        public string WordWrapBreakObtrusiveCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrapColumn")]
        public uint? WordWrapColumn { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wordWrapMinified")]
        public bool? WordWrapMinified { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        [JsonProperty("wrappingIndent")]
        public string WrappingIndent { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }

        // Construction Specific Properties Below
        // --------------------------------------

        [JsonProperty("autoIndent")]
        public bool? AutoIndent { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } } // = false;

        // public CssStyle ExtraEditorClassName?

        /// <summary>
        /// Gets or Sets the language of the Editor. This property is only read for initialization, changes should go through <see cref="CodeEditor.CodeLanguage"/>.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }

        //[JsonProperty("theme")]
        //public string Theme { get; set; } // = 'vs' (default), 'vs-dark', 'hc-black'

        // Value (initial value of model)

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            }); // TODO
        }

        private readonly Dictionary<string, object> _propertyBackingDictionary = new Dictionary<string, object>();

        private T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            object value;
            if (_propertyBackingDictionary.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        private bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            if (EqualityComparer<T>.Default.Equals(newValue, GetPropertyValue<T>(propertyName))) return false;

            _propertyBackingDictionary[propertyName] = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
    #pragma warning restore CS1591
}
