using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Monaco.Helpers
{
    // Simple Proxy to general CSS Line Styles
    public sealed class CssLineStyle : ICssStyle
    {
        public SolidColorBrush BackgroundColor { get; set; }
        public SolidColorBrush ForegroundColor { get; set; }

        public string Name { get; private set; }

        public CssLineStyle() {
            Name = CssStyleBroker.Instance.Register(this);
        }

        public string ToCss()
        {
            StringBuilder output = new StringBuilder(40);
            if (BackgroundColor != null)
            {
                output.AppendLine(String.Format("background: #{0:X2}{1:X2}{2:X2};", BackgroundColor.Color.R,
                                                                                    BackgroundColor.Color.G,
                                                                                    BackgroundColor.Color.B));
            }
            if (ForegroundColor != null)
            {
                output.AppendLine(String.Format("foreground: #{0:X2}{1:X2}{2:X2};", ForegroundColor.Color.R,
                                                                                    ForegroundColor.Color.G,
                                                                                    ForegroundColor.Color.B));
            }

            return CssStyleBroker.WrapCssClassName(this, output.ToString());
        }
    }
}
