using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Monaco.Languages
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.languages.ilanguageextensionpoint.html
    /// </summary>
    public sealed class ILanguageExtensionPoint
    {
        public string Id { get; private set; }
        public string[] Aliases { get; private set; }
        public string Configuration { get; private set; }
        public string[] Extensions { get; private set; }
        public string[] Filenames { get; private set; }
        public string[] FilenamePatterns { get; private set; }
        public string FirstLine { get; private set; }        
        public string[] Mimetypes { get; private set; }

        public static ILanguageExtensionPoint Create(IJsonValue languageValue)
        {
            ILanguageExtensionPoint language = new ILanguageExtensionPoint();
            var jsonObject = languageValue.GetObject();

            language.Id = jsonObject.GetNamedString("id");
            language.Aliases = jsonObject.GetNamedArray("aliases", new JsonArray()).Select(value => value.GetString()).ToArray();
            if (jsonObject.ContainsKey("configuration"))
            {
                language.Configuration = jsonObject.GetNamedString("configuration", string.Empty);
            }
            language.Extensions = jsonObject.GetNamedArray("extensions", new JsonArray()).Select(value => value.GetString()).ToArray();
            language.Filenames = jsonObject.GetNamedArray("filenames", new JsonArray()).Select(value => value.GetString()).ToArray();
            language.FilenamePatterns = jsonObject.GetNamedArray("filenamePatterns", new JsonArray()).Select(value => value.GetString()).ToArray();
            if (jsonObject.ContainsKey("firstLine"))
            {
                language.FirstLine = jsonObject.GetNamedString("firstLine", string.Empty);
            }
            language.Mimetypes = jsonObject.GetNamedArray("mimetypes", new JsonArray()).Select(value => value.GetString()).ToArray();

            return language;
        }
    }
}
