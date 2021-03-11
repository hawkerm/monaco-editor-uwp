﻿using Monaco.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace Monaco.Extensions
{
    internal static class WebViewExtensions
    {
        public static async Task RunScriptAsync(
            this WebView2 _view,
            string script,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await _view.RunScriptAsync<object>(script, member, file, line);
        }

        public static async Task<T> RunScriptAsync<T>(
            this WebView2 _view, 
            string script, 
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            var start = "try {\n";
            if (typeof(T) != typeof(object))
            {
                script = script.Trim(';');
                start += "JSON.stringify(" + script + ");";
            }
            else
            {
                start += script;
            }
            var fullscript = start + 
                "\n} catch (err) { JSON.stringify({ wv_internal_error: true, message: err.message, description: err.description, number: err.number, stack: err.stack }); }";

            if (_view.DispatcherQueue.HasThreadAccess)
            {
                try
                {
                    return await RunScriptHelperAsync<T>(_view, fullscript);
                }
                catch (Exception e)
                {
                    throw new JavaScriptExecutionException(member, file, line, script, e);
                }
            }
            else
            {
                return await _view.DispatcherQueue.RunTaskAsync(async () =>
                {
                    try
                    {
                        return await RunScriptHelperAsync<T>(_view, fullscript);
                    }
                    catch (Exception e)
                    {
                        throw new JavaScriptExecutionException(member, file, line, script, e);
                    }
                });
            }
        }

        private static async Task<T> RunScriptHelperAsync<T>(WebView2 _view, string script)
        {
            System.Diagnostics.Debug.WriteLine(script);
            System.Diagnostics.Debug.WriteLine("BEFORE");
            var returnstring = await _view.ExecuteScriptAsync(script);
            System.Diagnostics.Debug.WriteLine("AFTER");

            var s = JToken.Parse(returnstring).ToString();
            if (!string.IsNullOrEmpty(s))
            {
                JToken resultToken = JToken.Parse(s);

                if((resultToken == null) ||
                    (resultToken.Type == JTokenType.String && resultToken.ToString() == string.Empty) ||
                    (resultToken.Type == JTokenType.Null))
                {
                    return default(T);
                }

                if (resultToken.Type == JTokenType.Object)
                {
                    JObject result = (JObject)resultToken;

                    if (result.TryGetValue("wv_internal_error", out var wv_internal_error) && wv_internal_error.Type == JTokenType.Boolean && wv_internal_error.Value<bool>())
                    {
                        throw new JavaScriptInnerException(result["message"].Value<string>(), result["stack"].Value<string>());
                    }
                }
            }
            
            if (returnstring != null && returnstring != "null")
            {
                if (returnstring.StartsWith('"'))
                {
                    return JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject<string>(returnstring));
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(returnstring);
                }
            }

            return default;
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static async Task ExecuteScriptAsync(
            this WebView2 _view,
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await _view.ExecuteScriptAsync<object>(method, arg, serialize, member, file, line);
        }

        public static async Task ExecuteScriptAsync(
            this WebView2 _view,
            string method,
            object[] args,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await _view.ExecuteScriptAsync<object>(method, args, serialize, member, file, line);
        }

        public static async Task<T> ExecuteScriptAsync<T>(
            this WebView2 _view,
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            return await _view.ExecuteScriptAsync<T>(method, new object[] { arg }, serialize, member, file, line);
        }

        public static async Task<T> ExecuteScriptAsync<T>(
            this WebView2 _view,
            string method,
            object[] args,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            string[] sanitizedargs;

            if (serialize)
            {
                sanitizedargs = args.Select(item =>
                {
                    if (item is int || item is double)
                    {
                        return item.ToString();
                    }
                    else if (item is string)
                    {
                        return JsonConvert.ToString(item);
                    }
                    else
                    {
                        // TODO: Need JSON.parse?
                        return JsonConvert.SerializeObject(item, _settings);
                    }
                }).ToArray();
            }
            else
            {
                sanitizedargs = args.Select(item => item.ToString()).ToArray();
            }

            var script = method + "(" + string.Join(",", sanitizedargs) + ");";

            return await RunScriptAsync<T>(_view, script, member, file, line);
        }
    }

    internal sealed class JavaScriptExecutionException : Exception
    {
        public string Script { get; private set; }

        public string Member { get; private set; }

        public string FileName { get; private set; }

        public int LineNumber { get; private set; }

        public JavaScriptExecutionException(string member, string filename, int line, string script, Exception inner)
            : base("Error Executing JavaScript Code for " + member + "\nLine " + line + " of " + filename + "\n" + script + "\n", inner)
        {
            Member = member;
            FileName = filename;
            LineNumber = line;
            Script = script;
        }
    }

    internal sealed class JavaScriptInnerException : Exception
    {
        public string JavaScriptStackTrace { get; private set; } // TODO Use Enum of JS error types https://www.w3schools.com/js/js_errors.asp

        public JavaScriptInnerException(string message, string stack)
            : base(message)
        {
            JavaScriptStackTrace = stack;
        }
    }
}
