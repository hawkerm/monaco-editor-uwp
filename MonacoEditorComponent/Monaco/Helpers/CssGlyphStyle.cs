using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Helpers
{
    public sealed class CssGlyphStyle : ICssStyle
    {
        public Uri GlyphImage { get; set; }

        public string Name { get; private set; }

        public CssGlyphStyle()
        {
            Name = CssStyleBroker.Instance.Register(this);
        }

        public string ToCss()
        {
            return CssStyleBroker.WrapCssClassName(this, string.Format("background: url(\"{0}\");", GlyphImage.AbsoluteUri));
        }
    }
}
