using Monaco.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;

namespace Monaco
{
    /// <summary>
    /// Helper to static Monaco.Languages Namespace methods.
    /// https://microsoft.github.io/monaco-editor/api/modules/monaco.languages.html
    /// </summary>
    public sealed class LanguagesHelper
    {
        private Editor editor;

        public LanguagesHelper(Editor editor)
        {
            // We need the editor component in order to execute JavaScript within 
            // the WebView environment to retrieve data (even though this Monaco class is static).
            this.editor = editor;
        }

        public IAsyncOperation<IList<ILanguageExtensionPoint>> GetLanguagesAsync()
        {
            var json = editor.SendScriptAsync("JSON.stringify(monaco.languages.getLanguages())");

            return json.ContinueWith((result) =>
            {
                var jsonlanguages = JsonArray.Parse(result.Result);
                IList<ILanguageExtensionPoint> languages = new List<ILanguageExtensionPoint>(jsonlanguages.Count);
                for (int i = 0; i < jsonlanguages.Count; i++)
                {
                    languages.Add(ILanguageExtensionPoint.Create(jsonlanguages[i]));
                }

                return languages;
            }).AsAsyncOperation();
        }
    }
}
