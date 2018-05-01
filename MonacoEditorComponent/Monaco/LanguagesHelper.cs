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
        private WeakReference<CodeEditor> _editor;

        public LanguagesHelper(CodeEditor editor)
        {
            // We need the editor component in order to execute JavaScript within 
            // the WebView environment to retrieve data (even though this Monaco class is static).
            _editor = new WeakReference<CodeEditor>(editor);
        }

        public IAsyncOperation<IList<ILanguageExtensionPoint>> GetLanguagesAsync()
        {
            if (_editor.TryGetTarget(out CodeEditor editor))
            {
                return editor.SendScriptAsync<IList<ILanguageExtensionPoint>>("monaco.languages.getLanguages()").AsAsyncOperation();
            }

            return null;
        }
    }
}
