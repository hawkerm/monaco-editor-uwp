using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/classes/monaco.keymod.html
    /// </summary>
    public sealed class KeyMod
    {
        #pragma warning disable CS1591
        public static int WinCtrl => 256;
        public static int Alt => 512;
        public static int Shift => 1024;
        public static int CtrlCmd => 2048;

        public static int Chord(int firstPart, int secondPart)
        {
            // https://github.com/Microsoft/vscode/blob/master/src/vs/base/common/keyCodes.ts#L410
            var chordPart = ZeroFillRightShift((secondPart & 0x0000ffff) << 16, 0);
            return ZeroFillRightShift(firstPart | chordPart, 0);
        }
        #pragma warning restore CS1591

        // Info on Zero-Fill Right Shift http://www.vanguardsw.com/dphelp4/dph00369.htm
        // Supported natively by JavaScript, but not C#
        private static Int32 ZeroFillRightShift(Int32 i, Int32 j)
        {
            bool negativemask = (i < 0);
            i = i >> j;

            if (negativemask)
            {
                i &= 0x7FFFFFFF;
            }

            return i;
        }
    }
}
