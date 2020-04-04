using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.Foundation;

namespace Monaco.Editor
{
    /// <summary>
    /// https://github.com/Microsoft/vscode/blob/master/src/vs/editor/common/editorCommon.ts#L228
    /// </summary>
    public enum EndOfLinePreference
    {
        TextDefined = 0,
        LF = 1,
        CRLF = 2
    }

    /// <summary>
    /// https://github.com/Microsoft/vscode/blob/master/src/vs/editor/common/editorCommon.ts#L260
    /// </summary>
    public enum EndOfLineSequence
    {
        LF = 0,
        CRLF = 1
    }

    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodel.html
    /// </summary>
    public interface IModel
    {
        // TODO: Events

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        string Id { get; }
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        Uri Uri { get; }

        //IIdentifiedSingleEditOperation[] ApplyEditsAsync(IIdentifiedSingleEditOperation[] operations)
        //DeltaDecorationsAsync
        IAsyncAction DetectIndentationAsync(bool defaultInsertSpaces, bool defaultTabSize);
        //FindMatchesAsync(string searchString, bool searchOnlyEditableRange, bool isRegex, ...)
        //FindNextMatchAsync
        //FindPreviousMatchAsync
        //GetAllDecorationsAsync
        IAsyncOperation<uint> GetAlternativeVersionIdAsync();
        //GetDecorationOptionsAsync
        //GetDecorationRangeAsync
        //GetDecorationsInRangeAsync
        IAsyncOperation<string> GetEOLAsync();
        IAsyncOperation<Range> GetFullModelRangeAsync();
        IAsyncOperation<string> GetLineContentAsync(uint lineNumber);
        IAsyncOperation<uint> GetLineCountAsync();
        //GetLineDecorationsAsync
        IAsyncOperation<uint> GetLineFirstNonWhitespaceColumnAsync(uint lineNumber);
        IAsyncOperation<uint> GetLineLastNonWhitespaceColumnAsync(uint lineNumber);
        IAsyncOperation<uint> GetLineLengthAsync(uint lineNumber);
        IAsyncOperation<uint> GetLineMaxColumnAsync(uint lineNumber);
        IAsyncOperation<uint> GetLineMinColumnAsync(uint lineNumber);
        IAsyncOperation<IEnumerable<string>> GetLinesContentAsync();
        //GetLinesDecorationsAsync
        IAsyncOperation<string> GetModelIdAsync();
        IAsyncOperation<uint> GetOffsetAtAsync(IPosition position);
        IAsyncOperation<string> GetOneIndentAsync();
        //GetOptionsAsync
        IAsyncOperation<Position> GetPositionAtAsync(uint offset);
        IAsyncOperation<string> GetValueAsync();
        // TextDefined is default eol
        IAsyncOperation<string> GetValueAsync(EndOfLinePreference eol);
        IAsyncOperation<string> GetValueAsync(EndOfLinePreference eol, bool preserveBOM);
        IAsyncOperation<string> GetValueInRangeAsync(IRange range);
        IAsyncOperation<string> GetValueInRangeAsync(IRange range, EndOfLinePreference eol);
        IAsyncOperation<uint> GetValueLengthAsync();
        IAsyncOperation<uint> GetValueLengthAsync(EndOfLinePreference eol);
        IAsyncOperation<uint> GetValueLengthAsync(EndOfLinePreference eol, bool preserveBOM);
        IAsyncOperation<uint> GetValueLengthInRangeAsync(IRange range);
        IAsyncOperation<uint> GetVersionIdAsync();
        IAsyncOperation<WordAtPosition> GetWordAtPositionAsync(IPosition position);
        IAsyncOperation<WordAtPosition> GetWordUntilPositionAsync(IPosition position);
        IAsyncOperation<Position> ModifyPositionAsync(IPosition position, int number);
        IAsyncOperation<string> NormalizeIndentationAsync(string str);
        //PushEditOperationsAsync
        IAsyncAction PushStackElementAsync();
        IAsyncAction SetEOLAsync(EndOfLineSequence eol);
        IAsyncAction SetValue(string newValue);
        //IAsyncAction UpdateOptions(ITextModelUpdateOptions newOpts);
        IAsyncOperation<Position> ValidatePositionAsync(IPosition position);
        IAsyncOperation<Range> ValidateRangeAsync(IRange range);
    }
    }
