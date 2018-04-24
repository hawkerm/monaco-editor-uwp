using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco
{
    public sealed class Selection : IRange
    {
        [JsonProperty("startLineNumber")]
        public uint StartLineNumber { get; private set; }

        [JsonProperty("startColumn")]
        public uint StartColumn { get; private set; }

        [JsonProperty("endLineNumber")]
        public uint EndLineNumber { get; private set; }

        [JsonProperty("endColumn")]
        public uint EndColumn { get; private set; }

        [JsonProperty("positionLineNumber")]
        public uint PositionLineNumber { get; private set; }

        [JsonProperty("positionColumn")]
        public uint PositionColumn { get; private set; }

        [JsonProperty("selectionStartLineNumber")]
        public uint SelectionStartLineNumber { get; private set; }

        [JsonProperty("selectionStartColumn")]
        public uint SelectionStartColumn { get; private set; }

        [JsonIgnore]
        public SelectionDirection Direction { get; private set; }

        public Selection(uint selectionStartLineNumber, uint selectionStartColumn, uint positionLineNumber, uint positionColumn)
        {
            SelectionStartLineNumber = selectionStartLineNumber;
            SelectionStartColumn = selectionStartColumn;
            PositionLineNumber = positionLineNumber;
            PositionColumn = positionColumn;

            if (selectionStartLineNumber < positionLineNumber || (selectionStartLineNumber == positionLineNumber && selectionStartColumn <= positionColumn))
            {
                // Start is first
                StartLineNumber = SelectionStartLineNumber;
                StartColumn = SelectionStartColumn;
                EndLineNumber = PositionLineNumber;
                EndColumn = PositionColumn;

                Direction = SelectionDirection.LTR;
            }
            else
            {
                // Flipped
                StartLineNumber = PositionLineNumber;
                StartColumn = PositionColumn;
                EndLineNumber = SelectionStartLineNumber;
                EndColumn = SelectionStartColumn;

                Direction = SelectionDirection.RTL;
            }
        }

        public SelectionDirection GetDirection()
        {
            return Direction;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}-> {2}, {3}]", this.SelectionStartLineNumber, this.SelectionStartColumn, this.PositionLineNumber, this.PositionColumn);
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }

    public enum SelectionDirection
    {
        LTR,
        RTL
    }
}
