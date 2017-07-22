using System.Diagnostics;
using Windows.Foundation.Metadata;

namespace Monaco.Helpers
{
    [AllowForWeb]
    public sealed class DebugLogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
