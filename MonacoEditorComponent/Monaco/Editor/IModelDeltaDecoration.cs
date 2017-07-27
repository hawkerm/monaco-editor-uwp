using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Monaco.Editor
{
    /// <summary>
    /// Object Parser for https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodeldeltadecoration.html
    /// </summary>
    public sealed class IModelDeltaDecoration: IJsonable
    {
        public IModelDecorationOptions Options { get; private set; }
        public IRange Range { get; private set; }

        public IModelDeltaDecoration(IRange range, IModelDecorationOptions options)
        {
            this.Range = range;
            this.Options = options;
        }

        /*public static IModelDeltaDecoration Create(IJsonValue languageValue)
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
        }*/

        public string ToJson()
        {
            return String.Format("{{ \"range\": {0}, \"options\": {1} }}", Range.ToJson(), Options.ToJson());
        }
    }
}
