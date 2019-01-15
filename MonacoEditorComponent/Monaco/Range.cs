using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco
{
    /// <summary>
    /// Monaco Range in the editor. (startLineNumber,startColumn) is &lt;= (endLineNumber,endColumn)
    /// https://microsoft.github.io/monaco-editor/api/classes/monaco.range.html
    /// </summary>
    #pragma warning disable CS1591
    public sealed class Range : IRange
    {
        [JsonProperty("endColumn")]
        public uint EndColumn { get; private set; }
        [JsonProperty("endLineNumber")]
        public uint EndLineNumber { get; private set; }
        [JsonProperty("startColumn")]
        public uint StartColumn { get; private set; }
        [JsonProperty("startLineNumber")]
        public uint StartLineNumber { get; private set; }

        public Range(uint startLineNumber, uint startColumn, uint endLineNumber, uint endColumn)
        {
            // TODO: Range Check? Monaco doesn't seem to do it currently...
            StartLineNumber = startLineNumber;
            StartColumn = startColumn;
            EndLineNumber = endLineNumber;
            EndColumn = endColumn;
        }

        public Range CloneRange()
        {
            return new Range(StartLineNumber, StartColumn, EndLineNumber, EndColumn);
        }

        public Range CollapseToStart()
        {
            return new Range(StartColumn, StartColumn, StartLineNumber, StartColumn);
        }
        
        public Range ContainsPosition(IPosition position)
        {
            // TODO
            throw new NotImplementedException();
        }

        public bool ContainsRange(IRange range)
        {
            // TODO
            throw new NotImplementedException();
        }

        public bool EqualsRange(Range other)
        {
            return (this.StartColumn == other.StartColumn &&
                    this.StartLineNumber == other.StartLineNumber &&
                    this.EndColumn == other.EndColumn &&
                    this.EndLineNumber == other.EndLineNumber);
        }

        public Position GetEndPosition()
        {
            return new Position(this.EndLineNumber, this.EndColumn);
        }

        public Position GetStartPosition()
        {
            return new Position(this.StartLineNumber, this.StartColumn);
        }

        public Range IntersectRanges(IRange range)
        {
            // TODO
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            return this.StartLineNumber == this.EndLineNumber && this.StartColumn == this.EndColumn;
        }

        public Range PlusRange(IRange range)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Range SetEndPosition(uint endLineNumber, uint endColumn)
        {
            return new Range(this.StartLineNumber, this.StartColumn, endLineNumber, endColumn);
        }

        public Range SetStartPosition(uint startLineNumber, uint startColumn)
        {
            return new Range(startLineNumber, startColumn, this.EndLineNumber, this.EndColumn);
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}-> {2}, {3}]", this.StartLineNumber, this.StartColumn, this.EndLineNumber, this.EndColumn);
        }

        public string ToJson()
        {
            // Right json helper for this?  Or use JsonObject?  Or do I want a json lib dependency?
            return String.Format("{{ \"startLineNumber\": {0}, \"startColumn\": {1}, \"endLineNumber\": {2}, \"endColumn\": {3} }}", this.StartLineNumber, this.StartColumn, this.EndLineNumber, this.EndColumn);
        }
        
        public static Range Lift(IRange range)
        {
            return new Range(range.StartLineNumber, range.StartColumn, range.EndLineNumber, range.EndColumn);
        }

        // TODO: Weed out unique static method to put here.
    }
    #pragma warning restore CS1591
}
