using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation;

namespace Monaco.Editor
{
    /// <summary>
    /// Helper to access IModel interface methods off of CodeEditor object.
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodel.html
    /// </summary>
    public sealed class ModelHelper : IModel
    {
        private CodeEditor _editor;

        public ModelHelper(CodeEditor editor)
        {
            this._editor = editor;
        }

        public string Id => throw new NotImplementedException();

        public Uri Uri => throw new NotImplementedException();

        public IAsyncAction DetectIndentationAsync(bool defaultInsertSpaces, bool defaultTabSize)
        {
            return _editor.SendScriptAsync("model.detectIndentationAsync(" + JsonConvert.ToString(defaultInsertSpaces) +  ", " + JsonConvert.ToString(defaultTabSize) + ");").AsAsyncAction();
        }

        public IAsyncOperation<uint> GetAlternativeVersionIdAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getAlternativeVersionId());").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> GetEOLAsync()
        {
            return _editor.SendScriptAsync("model.getEOL();").AsAsyncOperation();
        }

        public IAsyncOperation<Range> GetFullModelRangeAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getFullModelRange());").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Range>(value) as Range;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> GetLineContentAsync(uint lineNumber)
        {
            return _editor.SendScriptAsync("model.getLineContent(" + lineNumber + ");").AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetLineCountAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLineCount());").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);
                
                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetLineFirstNonWhitespaceColumnAsync(uint lineNumber)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLineFirstNonWhitespaceColumn(" + lineNumber + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetLineLastNonWhitespaceColumnAsync(uint lineNumber)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLineLastNonWhitespaceColumn(" + lineNumber + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetLineMaxColumnAsync(uint lineNumber)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLineMaxColumn(" + lineNumber + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetLineMinColumnAsync(uint lineNumber)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLineMinColumn(" + lineNumber + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<IEnumerable<string>> GetLinesContentAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getLinesContent());").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    var array = JsonConvert.DeserializeObject<string[]>(value);
                    return array?.AsEnumerable();
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> GetModelIdAsync()
        {
            return _editor.SendScriptAsync("model.getModelId();").AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetOffsetAtAsync(IPosition position)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getOffsetAt(" + JsonConvert.SerializeObject(position) + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> GetOneIndentAsync()
        {
            return _editor.SendScriptAsync("model.getOneIndent();").AsAsyncOperation();
        }

        public IAsyncOperation<Position> GetPositionAtAsync(uint offset)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getPositionAt(" + offset + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Position>(value) as Position;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> GetValueAsync()
        {
            return _editor.SendScriptAsync("model.getValue();").AsAsyncOperation();
        }

        public IAsyncOperation<string> GetValueAsync(EndOfLinePreference eol)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<string> GetValueAsync(EndOfLinePreference eol, bool preserveBOM)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<string> GetValueInRangeAsync(IRange range)
        {
            return _editor.SendScriptAsync("model.getValueInRange(" + JsonConvert.SerializeObject(range) + ");").AsAsyncOperation();
        }

        public IAsyncOperation<string> GetValueInRangeAsync(IRange range, EndOfLinePreference eol)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<uint> GetValueLengthAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getValueLength());").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetValueLengthAsync(EndOfLinePreference eol)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<uint> GetValueLengthAsync(EndOfLinePreference eol, bool preserveBOM)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<uint> GetValueLengthInRangeAsync(IRange range)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getValueLengthInRange(" + JsonConvert.SerializeObject(range) + "));").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<uint> GetVersionIdAsync()
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getVersionId());").ContinueWith((result) =>
            {
                uint value = 0;

                uint.TryParse(result?.Result, out value);

                return value;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<IWordAtPosition> GetWordAtPositionAsync(IPosition position)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getWordAtPosition(" + JsonConvert.SerializeObject(position) + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<WordAtPosition>(value) as IWordAtPosition;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<IWordAtPosition> GetWordUntilPositionAsync(IPosition position)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.getWordUntilPosition(" + JsonConvert.SerializeObject(position) + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<WordAtPosition>(value) as IWordAtPosition;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<Position> ModifyPositionAsync(IPosition position, int number)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.modifyPosition(" + JsonConvert.SerializeObject(position) + ", " + number + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Position>(value) as Position;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<string> NormalizeIndentationAsync(string str)
        {
            return _editor.SendScriptAsync("model.normalizeIndentations(JSON.parse(" + JsonConvert.ToString(str) + "));").AsAsyncOperation();
        }

        public IAsyncAction PushStackElementAsync()
        {
            return _editor.SendScriptAsync("model.pushStackElement();").AsAsyncAction();
        }

        public IAsyncAction SetEOLAsync(EndOfLineSequence eol)
        {
            throw new NotImplementedException();
        }

        public IAsyncAction SetValue(string newValue)
        {
            return _editor.SendScriptAsync("model.setValue(JSON.parse(" + JsonConvert.ToString(newValue) + "));").AsAsyncAction();
        }

        public IAsyncOperation<Position> ValidatePositionAsync(IPosition position)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.validatePosition(" + JsonConvert.SerializeObject(position) + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Position>(value) as Position;
                }

                return null;
            }).AsAsyncOperation();
        }

        public IAsyncOperation<Range> ValidateRangeAsync(IRange range)
        {
            return _editor.SendScriptAsync("JSON.stringify(model.validateRange(" + JsonConvert.SerializeObject(range) + "));").ContinueWith((result) =>
            {
                var value = result?.Result;
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<Range>(value) as Range;
                }

                return null;
            }).AsAsyncOperation();
        }
    }
}
