using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Helpers
{
    public enum TextDecoration
    {
        None,
        Underline,
        Overline,
        LineThrough, // line-through
        Initial,
        Inherit
    }

    public sealed class CssInlineStyle: ICssStyle
    {
        public TextDecoration TextDecoration { get; set; }

        public string Name { get; private set; }

        public CssInlineStyle()
        {
            Name = CssStyleBroker.Instance.Register(this);
        }

        public string ToCss()
        {
            string text = TextDecoration.ToString().ToLower();
            if (TextDecoration == TextDecoration.LineThrough)
            {
                text = "line-through";
            }

            return CssStyleBroker.WrapCssClassName(this, String.Format("text-decoration: {0}", text));
        }
    }
}
