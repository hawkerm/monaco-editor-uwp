using Monaco.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;

namespace Monaco.Extensions
{
    internal static class WebViewExtensions
    {
        public static async Task<string> RunScriptAsync(
            this WebView _view, 
            string script, 
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            var fullscript = "try {\n" +
                                script +
                             "\n} catch (err) { JSON.stringify({ wv_internal_error: true, message: err.message, description: err.description, number: err.number, stack: err.stack }); }";

            if (_view.Dispatcher.HasThreadAccess)
            {
                try
                {
                    return await RunScriptHelperAsync(_view, fullscript);
                }
                catch (Exception e)
                {
                    throw new JavaScriptExecutionException(member, file, line, script, e);
                }
            }
            else
            {
                return await _view.Dispatcher.RunTaskAsync(async () =>
                {
                    try
                    {
                        return await RunScriptHelperAsync(_view, fullscript);
                    }
                    catch (Exception e)
                    {
                        throw new JavaScriptExecutionException(member, file, line, script, e);
                    }
                });
            }
        }

        private static async Task<string> RunScriptHelperAsync(WebView _view, string script)
        {            
            var returnstring = await _view.InvokeScriptAsync("eval", new string[] { script });

            if (JsonObject.TryParse(returnstring, out JsonObject result))
            {
                if (result.ContainsKey("wv_internal_error") && result["wv_internal_error"].ValueType == JsonValueType.Boolean && result["wv_internal_error"].GetBoolean())
                {
                    throw new JavaScriptInnerException(result["message"].GetString(), result["stack"].GetString());
                }
            }

            return returnstring;
        }
    }

    internal sealed class JavaScriptExecutionException : Exception
    {
        public string Script { get; private set; }

        public string Member { get; private set; }

        public string FileName { get; private set; }

        public int LineNumber { get; private set; }

        public JavaScriptExecutionException(string member, string filename, int line, string script, Exception inner)
            : base("Error Executing JavaScript Code", inner)
        {
            this.Member = member;
            this.FileName = filename;
            this.LineNumber = line;
            this.Script = script;
        }
    }

    internal sealed class JavaScriptInnerException : Exception
    {
        public string JavaScriptStackTrace { get; private set; } // TODO Use Enum of JS error types https://www.w3schools.com/js/js_errors.asp

        public JavaScriptInnerException(string message, string stack)
            : base(message)
        {
            this.JavaScriptStackTrace = stack;
        }
    }
}
