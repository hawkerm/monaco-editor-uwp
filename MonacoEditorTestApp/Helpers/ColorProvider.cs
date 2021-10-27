using Monaco;
using Monaco.Editor;
using Monaco.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Markup;

namespace MonacoEditorTestApp.Helpers
{
    public class ColorProvider : DocumentColorProvider
    {
        //// Called whenever Monaco needs to translate a color value to the textual representation (e.g. from the onhover color picker selector)
        public IAsyncOperation<IEnumerable<ColorPresentation>> ProvideColorPresentationsAsync(IModel document, ColorInformation colorInfo)
        {
            return AsyncInfo.Run(async delegate (CancellationToken cancelationToken)
            {
                return new ColorPresentation[]
                {
                    new ColorPresentation(colorInfo.Color.ToString()),
                }.AsEnumerable();
            });
        }

        //// Called whenever changes to the document are made, should identify colors in text and return the actual color value as well as where it was found in the text.
        public IAsyncOperation<IEnumerable<ColorInformation>> ProvideDocumentColorsAsync(IModel document)
        {
            return AsyncInfo.Run(async delegate (CancellationToken cancelationToken)
            {
                var info = new List<ColorInformation>();

                // Find all the 8 long hex values we can find in the document using regex.
                var matches = await document.FindMatchesAsync("#[A-Fa-f0-9]{8}", true, true, true, null, true);

                foreach (var match in matches)
                {
                    // Generate color info for each of these matches by using the XAML converter to read it to a Color value.
                    info.Add(new ColorInformation(XamlBindingHelper.ConvertValue(typeof(Color), match.Matches.First()) as Color? ?? Colors.Black, match.Range));
                }

                return info.AsEnumerable();
            });
        }
    }
}
