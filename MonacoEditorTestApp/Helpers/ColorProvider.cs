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
                var colorRange = new Range(19, 33, 19, 42); // TODO: This is just our hard reference to where we stored the color in the doc for now.

                var value = await document.GetValueInRangeAsync(colorRange);

                Color? color;

                color = XamlBindingHelper.ConvertValue(typeof(Color), value) as Color?;

                return new ColorInformation[]
                {
                    new ColorInformation(color ?? Colors.Red, colorRange)
                }.AsEnumerable();
            });
        }
    }
}
