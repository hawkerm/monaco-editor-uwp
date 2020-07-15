using Newtonsoft.Json;

namespace Monaco.Editor
{
    public sealed class ContextKey : IContextKey
    {
        [JsonIgnore]
        private readonly WinRT.WeakReference<CodeEditor> _editor;

        [JsonProperty("key")]
        public string Key { get; private set; }
        [JsonProperty("defaultValue")]
        public bool DefaultValue { get; private set; }
        [JsonProperty("value")]
        public bool Value { get; private set; }

        internal ContextKey(CodeEditor editor, string key, bool defaultValue)
        {
            _editor = new WinRT.WeakReference<CodeEditor>(editor);

            Key = key;
            DefaultValue = defaultValue;
        }

        private async void UpdateValueAsync()
        {
            if (_editor.TryGetTarget(out CodeEditor editor))
            {
                await editor.ExecuteScriptAsync("updateContext", new object[] { Key, Value });
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
    }
}
