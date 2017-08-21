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

        public bool? AcceptSuggestionOnCommitCharacter { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string AcceptSuggestionOnEnter { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string AccessibilitySupport { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string AriaLabel { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public bool? AutoClosingBrackets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? CodeLens { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? ContextMenu { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string CursorBlinking { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string CursorStyle { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public bool? DisableLayerHinting { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? DisableMonospaceOptimizations { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? DragAndDrop { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? EmptySelectionClipboard { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public IEditorFindOptions Find { get { return GetPropertyValue<IEditorFindOptions>(); } set { SetPropertyValue(value); } }
        public bool? FixedOverflowWidgets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? Folding { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string FontFamily { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public bool? FontLigatures { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public uint? FontSize { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public string FontWeight { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public bool? FormatOnPaste { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? GlyphMargin { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? HideCursorInOverviewRuler { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? Hover { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? IconsInSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public int? LetterSpacing { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public int? LineDecorationsWidth { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public int? LineHeight { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public string LineNumbers { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public int? LineNumbersMinChars { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public bool? Links { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? MatchBrackets { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public IEditorMinimapOptions Minimap { get { return GetPropertyValue<IEditorMinimapOptions>(); } set { SetPropertyValue(value); } }
        public int? MouseWheelScrollSensitivity { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public bool? MouseWheelZoom { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string MultiCursorModifier { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public bool? OccurrencesHighlight { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? OverviewRulerBorder { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public uint? OverviewRulerLanes { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public bool? ParameterHints { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? QuickSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public uint? QuickSuggestionsDelay { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public bool? ReadOnly { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? RenderControlCharacters { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? RenderIndentGuides { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string RenderLineHighlight { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string RenderWhitespace { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public uint? RevealHorizontalRightPadding { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public bool? RoundedSelection { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public uint[] Rulers { get { return GetPropertyValue<uint[]>(); } set { SetPropertyValue(value); } }
        public bool? ScrollBeyondLastLine { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public IEditorScrollbarOptions Scrollbar { get { return GetPropertyValue<IEditorScrollbarOptions>(); } set { SetPropertyValue(value); } }
        public bool? SelectOnLineNumbers { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? SelectionClipboard { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? SelectionHighlight { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string ShowFoldingControls { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string SnippetSuggestions { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public int? StopRenderingLineAfter { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public uint? SuggestFontSize { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public int? SuggestLineHeight { get { return GetPropertyValue<int?>(); } set { SetPropertyValue(value); } }
        public bool? SuggestOnTriggerCharacters { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? UseTabStops { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public bool? WordBasedSuggestions { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string WordSeparators { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string WordWrap { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string WordWrapBreakAfterCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string WordWrapBreakBeforeCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public string WordWrapBreakObtrusiveCharacters { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }
        public uint? WordWrapColumn { get { return GetPropertyValue<uint?>(); } set { SetPropertyValue(value); } }
        public bool? WordWrapMinified { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } }
        public string WrappingIndent { get { return GetPropertyValue<string>(); } set { SetPropertyValue(value); } }

        // Construction Specific Properties Below
        // --------------------------------------

        [JsonProperty("autoIndent")]
        public bool? AutoIndent { get { return GetPropertyValue<bool?>(); } set { SetPropertyValue(value); } } // = false;

        // public CssStyle ExtraEditorClassName?

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
