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
    /// https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.snippetstring.html
    /// https://code.visualstudio.com/docs/editor/userdefinedsnippets#_creating-your-own-snippets
    /// </summary>
    public sealed class SnippetString
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        public SnippetString(string svalue)
        {
            Value = svalue;
        }
    }

    public static class StringExtensions2
    {
        public static SnippetString ToSnippetString(this string svalue)
        {
            return new SnippetString(svalue);
        }
    }
}
