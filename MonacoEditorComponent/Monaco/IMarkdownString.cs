using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace Monaco
{
    /// <summary>
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.imarkdownstring.html
    /// </summary>
    public sealed class IMarkdownString
    {
        [JsonProperty("isTrusted")]
        public bool IsTrusted { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public IMarkdownString(string svalue) : this(svalue, false) { }

        public IMarkdownString(string svalue, bool isTrusted)
        {
            Value = svalue;
            IsTrusted = isTrusted;
        }
    }

    public static class StringExtensions
    {
        [DefaultOverload]
        public static IMarkdownString ToMarkdownString(this string svalue)
        {
            return ToMarkdownString(svalue, false);
        }

        [DefaultOverload]
        public static IMarkdownString ToMarkdownString(this string svalue, bool isTrusted)
        {
            return new IMarkdownString(svalue, isTrusted);
        }

        public static IMarkdownString[] ToMarkdownString([ReadOnlyArray] this string[] values)
        {
            return ToMarkdownString(values, false);
        }

        public static IMarkdownString[] ToMarkdownString([ReadOnlyArray] this string[] values, bool isTrusted)
        {
            return values.Select(value => new IMarkdownString(value, isTrusted)).ToArray();
        }
    }
}
