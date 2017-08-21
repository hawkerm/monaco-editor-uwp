using Monaco.Helpers;
using Monaco.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Newtonsoft.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditoroptions.html
    /// </summary>
    #pragma warning disable CS1591
    public interface IEditorOptions
    {
        [JsonProperty("acceptSuggestionOnCommitCharacter")]
        bool? AcceptSuggestionOnCommitCharacter { get; set; } // = true;
        [JsonProperty("acceptSuggestionOnEnter")]
        string AcceptSuggestionOnEnter { get; set; } // = "on"; // on, smart, off
        [JsonProperty("accessibilitySupport")]
        string AccessibilitySupport { get; set; } // = "auto"; // auto, off, on
        [JsonProperty("ariaLabel")]
        string AriaLabel { get; set; }
        [JsonProperty("autoClosingBrackets")]
        bool? AutoClosingBrackets { get; set; } // = true;
        //bool AutomaticLayout { get; } // We handle this smartly as part of being a control, don't think it's needed to be exposed.
        [JsonProperty("codeLens")]
        bool? CodeLens { get; set; } // = true;
        [JsonProperty("contextMenu")]
        bool? ContextMenu { get; set; } // = true;
        [JsonProperty("cursorBlinking")]
        string CursorBlinking { get; set; } // = "blink"; // blink, smooth, phase, expand, solid
        [JsonProperty("cursorStyle")]
        string CursorStyle { get; set; } // = "line"; // line, block
        [JsonProperty("disableLayerHinting")]
        bool? DisableLayerHinting { get; set; }
        [JsonProperty("disableMonospaceOptimizations")]
        bool? DisableMonospaceOptimizations { get; set; }
        [JsonProperty("dragAndDrop")]
        bool? DragAndDrop { get; set; }
        [JsonProperty("emptySelectionClipboard")]
        bool? EmptySelectionClipboard { get; set; } // = true;
        //string ExtraEditorClassName { get; set; } // CSS Class?
        [JsonProperty("find")]
        IEditorFindOptions Find { get; set; } // = new IEditorFindOptions();
        [JsonProperty("fixedOverflowWidgets")]
        bool? FixedOverflowWidgets { get; set; }
        [JsonProperty("folding")]
        bool? Folding { get; set; }
        [JsonProperty("fontFamily")]
        string FontFamily { get; set; }
        [JsonProperty("fontLigatures")]
        bool? FontLigatures { get; set; }
        [JsonProperty("fontSize")]
        uint? FontSize { get; set; }
        [JsonProperty("fontWeight")]
        string FontWeight { get; set; } // = "normal"; // 'normal' | 'bold' | 'bolder' | 'lighter' | 'initial' | 'inherit' | '100' | '200' | '300' | '400' | '500' | '600' | '700' | '800' | '900';
        [JsonProperty("formatOnPaste")]
        bool? FormatOnPaste { get; set; }
        [JsonProperty("glyphMargin")]
        bool? GlyphMargin { get; set; }
        [JsonProperty("hideCursorInOverviewRuler")]
        bool? HideCursorInOverviewRuler { get; set; }
        [JsonProperty("hover")]
        bool? Hover { get; set; } // = true;
        [JsonProperty("iconsInSuggestions")]
        bool? IconsInSuggestions { get; set; } // = true;
        [JsonProperty("letterSpacing")]
        int? LetterSpacing { get; set; }
        [JsonProperty("lineDecorationsWidth")]
        int? LineDecorationsWidth { get; set; } // = 10; // TODO: Figure out support for union
        [JsonProperty("lineHeight")]
        int? LineHeight { get; set; }
        [JsonProperty("lineNumbers")]
        string LineNumbers { get; set; } // = "on"; // off, relative, function // TODO: Figure out function support for line numbers, probably separate add-on to interface
        [JsonProperty("lineNumbersMinChars")]
        int? LineNumbersMinChars { get; set; } //= 5;
        [JsonProperty("links")]
        bool? Links { get; set; } // = true;
        [JsonProperty("matchBrackets")]
        bool? MatchBrackets { get; set; } // = true;
        [JsonProperty("minimap")]
        IEditorMinimapOptions Minimap { get; set; }
        [JsonProperty("mouseWheelScrollSensitivity")]
        int? MouseWheelScrollSensitivity { get; set; } // = 1;
        [JsonProperty("mouseWheelZoom")]
        bool? MouseWheelZoom { get; set; }
        [JsonProperty("multiCursorModifier")]
        string MultiCursorModifier { get; set; } // = "alt"; ctrlCmd
        [JsonProperty("occurrencesHighlight")]
        bool? OccurrencesHighlight { get; set; } // = true;
        [JsonProperty("overviewRulerBorder")]
        bool? OverviewRulerBorder { get; set; } // = true;
        [JsonProperty("overviewRulerLanes")]
        uint? OverviewRulerLanes { get; set; } // = 2;
        [JsonProperty("parameterHints")]
        bool? ParameterHints { get; set; } // default?
        [JsonProperty("quickSuggestions")]
        bool? QuickSuggestions { get; set; } // = true; not sure what object option is...
        [JsonProperty("quickSuggestionsDelay")]
        uint? QuickSuggestionsDelay { get; set; } // = 500; (ms)
        [JsonProperty("readOnly")]
        bool? ReadOnly { get; set; }
        [JsonProperty("renderControlCharacters")]
        bool? RenderControlCharacters { get; set; }
        [JsonProperty("renderIndentGuides")]
        bool? RenderIndentGuides { get; set; }
        [JsonProperty("renderLineHighlight")]
        string RenderLineHighlight { get; set; } // = "all"; none, gutter, line
        [JsonProperty("renderWhitespace")]
        string RenderWhitespace { get; set; } // = "none"; boundary, all
        [JsonProperty("revealHorizontalRightPadding")]
        uint? RevealHorizontalRightPadding { get; set; } // = 30; (px)
        [JsonProperty("roundedSelection")]
        bool? RoundedSelection { get; set; } // = true;
        [JsonProperty("rulers")]
        uint[] Rulers { get; set; }
        [JsonProperty("scrollBeyondLastLine")]
        bool? ScrollBeyondLastLine { get; set; } // = true;
        [JsonProperty("scrollbar")]
        IEditorScrollbarOptions Scrollbar { get; set; }
        [JsonProperty("selectOnLineNumbers")]
        bool? SelectOnLineNumbers { get; set; } // = true;
        [JsonProperty("selectionClipboard")]
        bool? SelectionClipboard { get; set; } // = true;
        [JsonProperty("selectionHighlight")]
        bool? SelectionHighlight { get; set; } // = true;
        [JsonProperty("showFoldingControls")]
        string ShowFoldingControls { get; set; } // = "mouseover"; always
        [JsonProperty("snipperSuggestions")]
        string SnippetSuggestions { get; set; } // = "true"; top, bottom, inline, none
        [JsonProperty("stopRenderingLineAfter")]
        int? StopRenderingLineAfter { get; set; } // = 10000;
        [JsonProperty("suggestFontSize")]
        uint? SuggestFontSize { get; set; }
        [JsonProperty("suggestLineHeight")]
        int? SuggestLineHeight { get; set; }
        [JsonProperty("suggestOnTriggerCharacters")]
        bool? SuggestOnTriggerCharacters { get; set; } // = true;
        [JsonProperty("useTabStops")]
        bool? UseTabStops { get; set; } // default?
        [JsonProperty("wordBasedSuggestions")]
        bool? WordBasedSuggestions { get; set; } // = true;
        [JsonProperty("wordSeparators")]
        string WordSeparators { get; set; } // A string containing the word separators used when doing word navigation. Defaults to `~!@#$%^&*()-=+[{]}\|;:\'",.<>/?
        [JsonProperty("wordWrap")]
        string WordWrap { get; set; } // = "off"; on, wordWrapColumn, bounded
        [JsonProperty("wordWrapBreakAfterCharacters")]
        string WordWrapBreakAfterCharacters { get; set; } // Configure word wrapping characters. A break will be introduced after these characters. Defaults to ' \t})]?|&,;'
        [JsonProperty("wordWrapBeforeCharacters")]
        string WordWrapBreakBeforeCharacters { get; set; } // Configure word wrapping characters. A break will be introduced before these characters. Defaults to '{([+'
        [JsonProperty("wordWrapObtrusiveCharacters")]
        string WordWrapBreakObtrusiveCharacters { get; set; } // Configure word wrapping characters. A break will be introduced after these characters only if no wordWrapBreakBeforeCharacters or wordWrapBreakAfterCharacters were found. Defaults to '.'
        [JsonProperty("wordWrapColumn")]
        uint? WordWrapColumn { get; set; } // = 80;
        [JsonProperty("wordWrapMinified")]
        bool? WordWrapMinified { get; set; }
        [JsonProperty("wrappingIndent")]
        string WrappingIndent { get; set; } // = "none"; same, indent
    }
    #pragma warning restore CS1591
}
