using Monaco.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Editor
{
    #pragma warning disable CS1591
    public sealed class ContextKey : IContextKey
    {
        [JsonIgnore]
        private WeakReference<CodeEditor> _editor;

        [JsonProperty("key")]
        public string Key { get; private set; }
        [JsonProperty("defaultValue")]
        public bool DefaultValue { get; private set; }
        [JsonProperty("value")]
        public bool Value { get; private set; }

        internal ContextKey(CodeEditor editor, string key, bool defaultValue)
        {
            _editor = new WeakReference<CodeEditor>(editor);

            Key = key;
            DefaultValue = defaultValue;
        }

        private async void UpdateValueAsync()
        {
            if (_editor.TryGetTarget(out CodeEditor editor))
            {
                await editor.InvokeScriptAsync("updateContext", new object[] { Key, Value });
            }
        }

        public bool Get()
        {
            return Value;
        }

        public void Reset()
        {
            Value = DefaultValue;

            UpdateValueAsync();
        }

        public void Set(bool value)
        {
            Value = value;

            UpdateValueAsync();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    #pragma warning restore CS1591
}
