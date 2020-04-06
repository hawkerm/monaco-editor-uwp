using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Monaco.Editor
{
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

        /// <summary>
        /// Search the model.
        /// </summary>
        /// <returns>
        /// The ranges where the matches are. It is empty if not matches have been found.
        /// 
        /// </returns>
        [DefaultOverload]
        IAsyncOperation<IEnumerable<FindMatch>> FindMatchesAsync(string searchString, bool searchOnlyEditableRange, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches);

        [DefaultOverload]
        IAsyncOperation<IEnumerable<FindMatch>> FindMatchesAsync(string searchString, bool searchOnlyEditableRange, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches, double limitResultCount);

        /// <summary>
        /// Search the model.
        /// </summary>
        /// <returns>
        /// The ranges where the matches are. It is empty if no matches have been found.
        /// 
        /// </returns>
        IAsyncOperation<IEnumerable<FindMatch>> FindMatchesAsync(string searchString, IRange searchScope, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches);

        IAsyncOperation<IEnumerable<FindMatch>> FindMatchesAsync(string searchString, IRange searchScope, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches, double limitResultCount);

        /// <summary>
        /// Search the model for the next match. Loops to the beginning of the model if needed.
        /// </summary>
        /// <returns>
        /// The range where the next match is. It is null if no next match has been found.
        /// 
        /// </returns>
        IAsyncOperation<FindMatch> FindNextMatchAsync(string searchString, IPosition searchStart, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches);

        /// <summary>
        /// Search the model for the previous match. Loops to the end of the model if needed.
        /// </summary>
        /// <returns>
        /// The range where the previous match is. It is null if no previous match has been found.
        /// 
        /// </returns>
        IAsyncOperation<FindMatch> FindPreviousMatchAsync(string searchString, IPosition searchStart, bool isRegex, bool matchCase, string wordSeparators, bool captureMatches);

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
