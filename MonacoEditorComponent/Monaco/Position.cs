using Newtonsoft.Json;
using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Monaco
{
    /// <summary>
    /// A position in the editor.
    /// https://microsoft.github.io/monaco-editor/api/classes/monaco.position.html
    /// </summary>
    public sealed class Position : IPosition
    {
        // TODO: Investigate why with .NET Native the interface attributes aren't carried forward?
        [JsonProperty("column")]
        public uint Column { get; private set; }

        [JsonProperty("lineNumber")]
        public uint LineNumber { get; private set; }

        public Position(uint lineNumber, uint column)
        {
            Column = column;
            LineNumber = lineNumber;
        }

        public Position Clone()
        {
            return new Position(LineNumber, Column);
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                var other = obj as Position;
                return LineNumber == other.LineNumber && Column == other.Column;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new Point(LineNumber, Column).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", LineNumber, Column);
        }

        public bool IsBefore(Position other)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public bool IsBeforeOrEqual(Position other)
        {
            // TODO:
            throw new NotImplementedException();
        }

        [DefaultOverload]
        public int CompareTo(Position other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if (obj is IPosition)
            {
                return CompareTo(Position.Lift(obj as IPosition));
            }

            throw new NotImplementedException();
        }

        public static int Compare(Position a, Position b)
        {
            return a.CompareTo(b);
        }

        // Can't Export static Method with same name in Windows Runtime Component
        /*public static bool Equals(Position a, Position b)
        {
            return a.Equals(b);
        }

        public static bool IsBefore(Position a, Position b)
        {
            return a.IsBefore(b);
        }

        public static bool IsBeforeOrEqual(Position a, Position b)
        {
            return a.IsBeforeOrEqual(b);
        }*/

        public static bool IsIPosition(object a)
        {
            return a is Position;
        }

        public static Position Lift(IPosition pos)
        {
            return new Position(pos.LineNumber, pos.Column);
        }

        public string ToJson()
        {
            return string.Format("{{ \"lineNumber\": {0}, \"column\": {1} }}", LineNumber, Column);
        }
    }
}
