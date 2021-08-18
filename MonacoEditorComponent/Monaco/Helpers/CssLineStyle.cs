﻿using Newtonsoft.Json;
using System;
using System.Text;
using Windows.UI.Xaml.Media;

namespace Monaco.Helpers
{
    /// <summary>
    /// Simple Proxy to general CSS Line Styles.
    /// Line styles are overlayed behind text in the editor and are useful for highlighting sections of text efficiently
    /// </summary>
    [JsonConverter(typeof(CssStyleConverter))]
    public sealed class CssLineStyle : ICssStyle
    {
        public SolidColorBrush BackgroundColor { get; set; }
        [Obsolete("Use ForegroundColor on CssInlineStyle instead, this is an overlay.")]
        public SolidColorBrush ForegroundColor { get; set; }

        public uint Id { get; }

        public string Name { get; }

        public CssLineStyle()
        {
            Id = CssStyleBroker.Register(this);
            Name = "generated-style-" + Id;
        }

        public string ToCss()
        {
            StringBuilder output = new StringBuilder(40);
            if (BackgroundColor != null)
            {
                output.AppendLine(string.Format("background: #{0:X2}{1:X2}{2:X2};", BackgroundColor.Color.R,
                                                                                    BackgroundColor.Color.G,
                                                                                    BackgroundColor.Color.B));
            }
#pragma warning disable CS0618 // Type or member is obsolete
            if (ForegroundColor != null)
            {
                output.AppendLine(string.Format("color: #{0:X2}{1:X2}{2:X2} !important;", ForegroundColor.Color.R,
                                                                               ForegroundColor.Color.G,
                                                                               ForegroundColor.Color.B));
            }
#pragma warning restore CS0618 // Type or member is obsolete

            return this.WrapCssClassName(output.ToString());
        }
    }
}
